using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    #region State

    [SerializeField] UWeaponData m_WeaponData;

    #endregion

    #region Property

    public UWeaponData WeaponData => m_WeaponData;

    #endregion
    
    // 에디터 설정
    [SerializeField] int m_WeaponID;
    [SerializeField] GameObject m_BulletPrefab;
    [SerializeField] int m_BaseDamage;
    [SerializeField] int m_Damage;
    [SerializeField] int m_Count;
    [SerializeField] float m_BaseSpeed;
    [SerializeField] float m_Speed;

    [SerializeField] int m_ProjectileNum; 
    
    // 프로퍼티
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

    // 상태
    float m_Timer;
    
    // 컴포넌트
    Player m_Player;
    Scanner m_Scanner;

    void onGameOver_Event()
    {
        Destroy(gameObject);
    }

    void OnGameExit_Event()
    {
        Destroy(gameObject);
    }

    void Awake()
    {
        m_Player = SurvivalGameManager.Get().GetPlayer();
    }

    void OnEnable()
    {
        var gameManager = SurvivalGameManager.Get();

        gameManager.OnGameOver += onGameOver_Event;
        gameManager.OnGameExit += OnGameExit_Event;
    }

    void OnDisable()
    {
        var gameManager = SurvivalGameManager.Get();
        
        gameManager.OnGameOver -= onGameOver_Event;
        gameManager.OnGameExit -= OnGameExit_Event;
    }

    void Update()
    {
        switch (m_WeaponData.WeaponType)
        {
            case EWeaponType.Melee:
                transform.Rotate(m_Speed * SurvivalGameState.Get().GetStatsComponent().TotalStats.AttackSpeed * Time.deltaTime * Vector3.back);
                break;
            default:
                m_Timer += Time.deltaTime;
                if (m_Timer > m_Speed / SurvivalGameState.Get().GetStatsComponent().TotalStats.AttackSpeed)
                {
                    m_Timer = 0;
                    Fire();
                }
                break;
        }
    }

    #region API

    public void Init(UWeaponData _weaponData)
    {
        // Weapon Data 참조 전달
        m_WeaponData = _weaponData;

        // Basic Set
        name = "Weapon " + m_WeaponData.DisplayName;
        transform.parent = m_Player.transform;
        transform.localPosition = Vector3.zero;
        m_Scanner = GetComponentInParent<Scanner>(); // TODO 리팩토링 필요
        m_BulletPrefab = m_WeaponData.Prefab;
        
        ApplyWeaponAttribute();
    }

    public void LevelUp()
    {
        m_WeaponData.Level++;
        ApplyWeaponAttribute();
        foreach (var gear in GetComponents<Gear>())
        {
            gear.ApplyGear();
        }
    }

    #endregion

    #region Method

    public void ApplyWeaponAttribute()
    {
        var weaponAttribute = m_WeaponData.Attribute;

        m_BaseDamage = weaponAttribute.Damage;

        SpriteRenderer hand;
        
        switch (m_WeaponData.WeaponType)
        {
            case EWeaponType.Melee:
                m_BaseSpeed = weaponAttribute.Speed;
                m_Count = weaponAttribute.ProjectileNum;
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

    #endregion

    void Arrange()
    {
        for (int i = 0; i < m_Count; i++)
        {
            GameObject bullet;
            if (i < transform.childCount)
                bullet = transform.GetChild(i).gameObject;
            else
            {
                // Bullet 스폰 및 배치
                bullet = PoolManager.GetInstance(m_BulletPrefab);
                bullet.transform.parent = transform;
            }
            
            bullet.transform.localPosition = Vector3.zero;
            bullet.transform.localRotation = Quaternion.identity;

            Vector3 rot = 360f * i / m_Count * Vector3.forward;
            bullet.transform.Rotate(rot);
            bullet.transform.Translate(bullet.transform.up * 1.5f, Space.World);
            
            // Bullet 설정
            bullet.GetComponent<Bullet>().Init(m_Damage, -100, Vector3.zero, m_WeaponData.BulletSprite); // -1은 무한 관통
            
            // Bullet 활성화
            bullet.SetActive(true); // 나중에 IObjectPool에 Finish 추가 예정
        }
    }

    void Fire()
    {
        // 목표물 확인
        if (!m_Scanner.MainTarget) return;
        
        // 타깃 방향
        Vector3 position = transform.position;
        Vector3 dir = (m_Scanner.MainTarget.position - position).normalized;
        
        // 90도 범위 내에서 발사
        // 기본 방향을 타깃방향에서 -45도로 offset 설정
        Vector3 startDir = Quaternion.AngleAxis(-45, Vector3.forward) * dir;
        float intervalAngle = 90f / (m_ProjectileNum + 1);
        Vector3 fireDir;
        
        // Bullet 스폰
        for (int i = 1; i <= m_ProjectileNum; i++)
        {
            Bullet bullet = PoolManager.GetInstance<Bullet>(m_BulletPrefab);
            fireDir = Quaternion.AngleAxis(i * intervalAngle, Vector3.forward) * startDir; 
            
            // Bullet 방향 및 속도 설정
            bullet.transform.position = position;
            bullet.transform.rotation = Quaternion.FromToRotation(Vector3.up, fireDir);
            bullet.Init(m_Damage, m_Count, fireDir, m_WeaponData.BulletSprite);

            // Bullet 활성화
            bullet.gameObject.SetActive(true);
        }
        
        AudioManager.Get().PlaySfx(AudioManager.Sfx.Range);
    }
}
