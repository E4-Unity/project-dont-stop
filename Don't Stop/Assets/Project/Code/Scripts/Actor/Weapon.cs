using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // 에디터 설정
    [SerializeField] int m_WeaponID;
    [SerializeField] GameObject m_BulletPrefab;
    [SerializeField] int m_Damage;
    [SerializeField] int m_Count;
    [SerializeField] float m_Speed;
    
    // 프로퍼티
    public int WeaponID => m_WeaponID;

    public float Speed
    {
        get => m_Speed;
        set => m_Speed = value;
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
        switch (m_WeaponID)
        {
            case 0:
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

    public void LevelUp(int _damage, int _count)
    {
        m_Damage = (int)(_damage * (1 + SurvivalGameState.Get().GetStatsComponent().TotalStats.Attack / 100.0f));
        m_Count += _count;
        
        if(m_WeaponID == 0)
            Arrange();
        
        m_Player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    public void Init(ItemData _data)
    {
        // Basic Set
        name = "Weapon " + _data.ItemID;
        transform.parent = m_Player.transform;
        transform.localPosition = Vector3.zero;
        m_Scanner = GetComponentInParent<Scanner>(); // TODO 리팩토링 필요
        m_BulletPrefab = _data.Projectile;
        
        // Property Set
        m_WeaponID = _data.ItemID;
        
        // 데미지 계산식 : WeaponDamage * (Total Attack / 100)
        m_Damage = (int)(_data.BaseDamage * (1 + SurvivalGameState.Get().GetStatsComponent().TotalStats.Attack / 100.0f));
        m_Count = _data.BaseCount + Character.Count;

        switch (m_WeaponID)
        {
            case 0:
                m_Speed = 100f;
                Arrange();
                break;
            default:
                m_Speed = 1f;
                break;
        }

        // Hand Set
        // TODO OnEquip 델리게이트로 Player에서 직접 할당 예정
        var hand = m_Player.GetHands()[(int)_data.Type];
        hand.sprite = _data.WeaponSprite;
        hand.gameObject.SetActive(true);
        
        m_Player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

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
            bullet.GetComponent<Bullet>().Init(m_Damage, -100, Vector3.zero); // -1은 무한 관통
            
            // Bullet 활성화
            bullet.SetActive(true); // 나중에 IObjectPool에 Finish 추가 예정
        }
    }

    void Fire()
    {
        // 목표물 확인
        if (!m_Scanner.MainTarget) return;
        
        // Bullet 스폰
        Bullet bullet = PoolManager.GetInstance<Bullet>(m_BulletPrefab);
        
        // Bullet 방향 및 속도 설정
        Vector3 position = transform.position;
        Vector3 dir = (m_Scanner.MainTarget.position - position).normalized;
        bullet.transform.position = position;
        bullet.transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        bullet.Init(m_Damage, m_Count, dir);

        // Bullet 활성화
        bullet.gameObject.SetActive(true);
        
        AudioManager.Get().PlaySfx(AudioManager.Sfx.Range);
    }
}
