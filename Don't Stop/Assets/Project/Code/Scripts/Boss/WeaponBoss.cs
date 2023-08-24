using System;
using UnityEngine;

public class WeaponBoss : MonoBehaviour
{
    [SerializeField] UWeaponData m_WeaponData;

    public UWeaponData WeaponData => m_WeaponData;

    [SerializeField] int m_WeaponID;
    [SerializeField] GameObject m_BulletPrefab;
    [SerializeField] int m_BaseDamage;
    [SerializeField] int m_Damage;
    [SerializeField] int m_Count;
    [SerializeField] float m_BaseSpeed;
    [SerializeField] float m_Speed;
    [SerializeField] int m_ProjectileNum;

    public int WeaponID => m_WeaponID;
    public float BaseSpeed => m_BaseSpeed;
    public int BaseDamage => m_BaseDamage;

    public int Damage
    {
        get => m_Damage;
        set
        {
            m_Damage = value;
            if (m_WeaponData.WeaponType == EWeaponType.Melee)
            {
                Arrange();
            }
        }
    }

    public float Speed
    {
        get => m_Speed;
        set
        {
            m_Speed = value;
            if (m_WeaponData.WeaponType == EWeaponType.Melee)
            {
                Arrange();
            }
        }
    }

    float m_Timer;

    PlayerBoss m_Player;
    Scanner m_Scanner;

    void Awake()
    {
        m_Player = GameManagerBoss.Get().GetPlayer();
    }

    void Update()
    {
        m_Player = GameManagerBoss.Get().GetPlayer();
        switch (m_WeaponData.WeaponType)
        {
            case EWeaponType.Melee:
                transform.Rotate(m_Speed * m_Player.GetComponent<PlayerBoss>().attackSpeed * Time.deltaTime * Vector3.back);
                break;
            default:
                m_Timer += Time.deltaTime;
                if (m_Timer > 1 / m_Player.GetComponent<PlayerBoss>().attackSpeed)
                {
                    m_Timer = 0;
                    Fire();
                }
                break;
        }
    }

    public void Init(UWeaponData _weaponData)
    {
        m_WeaponData = _weaponData;

        name = "Weapon " + m_WeaponData.DisplayName;
        transform.parent = m_Player.transform;
        transform.localPosition = Vector3.zero;
        m_Scanner = GetComponentInParent<Scanner>();
        m_BulletPrefab = m_WeaponData.Prefab;

        ApplyWeaponAttribute();
    }

    public void LevelUp()
    {
        m_WeaponData.Level++;
        ApplyWeaponAttribute();
        foreach (var gear in GetComponents<GearBoss>())
        {
            gear.ApplyGear();
        }
    }

    public void ApplyWeaponAttribute()
    {
        var weaponAttribute = m_WeaponData.Attribute;

        m_BaseDamage = weaponAttribute.Damage;

        SpriteRenderer hand;

        switch (m_WeaponData.WeaponType)
        {
            case EWeaponType.Melee:
                m_BaseSpeed = weaponAttribute.Speed;
                m_ProjectileNum = weaponAttribute.ProjectileNum;
                hand = m_Player.GetHands()[0];
                Arrange();
                break;
            case EWeaponType.Range:
                m_BaseSpeed = weaponAttribute.AttackRate;
                m_Count = weaponAttribute.Penetration - 1;
                m_ProjectileNum = weaponAttribute.ProjectileNum;
                hand = m_Player.GetHands()[1];
                break;
            default:
                hand = m_Player.GetHands()[0];
                break;
        }

        hand.sprite = m_WeaponData.DisplaySprite;
        hand.gameObject.SetActive(true);
    }

    void Arrange()
    {
        for (int i = 0; i < m_ProjectileNum; i++)
        {
            GameObject bullet;
            if (i < transform.childCount)
                bullet = transform.GetChild(i).gameObject;
            else
            {
                bullet = PoolManager.GetInstance(m_BulletPrefab);
                bullet.transform.parent = transform;
            }

            bullet.transform.localPosition = Vector3.zero;
            bullet.transform.localRotation = Quaternion.identity;

            Vector3 rot = 360f * i / m_ProjectileNum * Vector3.forward;
            bullet.transform.Rotate(rot);
            bullet.transform.Translate(bullet.transform.up * 1.5f, Space.World);

            bullet.GetComponent<Bullet>().Init(m_Damage, -100, Vector3.zero, m_WeaponData.BulletSprite);

            bullet.SetActive(true);
        }
    }

    void Fire()
    {
        if (!m_Scanner.MainTarget) return;

        Vector3 position = transform.position;
        Vector3 dir = (m_Scanner.MainTarget.position - position).normalized;

        Vector3 startDir = Quaternion.AngleAxis(-45, Vector3.forward) * dir;
        float intervalAngle = 90f / (m_ProjectileNum + 1);
        Vector3 fireDir;

        for (int i = 1; i <= m_ProjectileNum; i++)
        {
            Bullet bullet = PoolManager.GetInstance<Bullet>(m_BulletPrefab);
            fireDir = Quaternion.AngleAxis(i * intervalAngle, Vector3.forward) * startDir;

            bullet.transform.position = position;
            bullet.transform.rotation = Quaternion.FromToRotation(Vector3.up, fireDir);
            bullet.Init(m_Damage, m_Count, fireDir, m_WeaponData.BulletSprite);

            bullet.gameObject.SetActive(true);
        }

        AudioManager.Get().PlaySfx(AudioManager.Sfx.Range);
    }
}
