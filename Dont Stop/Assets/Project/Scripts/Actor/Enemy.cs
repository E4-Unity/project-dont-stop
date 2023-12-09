using System.Collections;
using Framework;
using UnityEngine;
using UnityEngine.U2D.Animation;
using Random = UnityEngine.Random;

public class Enemy : Actor
{
    #region Components

    TimeManager m_TimeManager;
    Rigidbody2D m_Rigidbody;
    SpriteRenderer m_SpriteRenderer;
    SpriteLibrary m_SpriteLibrary;
    Animator m_Animator;
    Collider2D m_Collider;

    #endregion
    
    #region Reference
    
    [SerializeField] EnemyData[] m_EnemyData;
    [SerializeField] GameObject m_GoldPrefab;
    [SerializeField] GameObject m_FloatingTextDamage;

    #endregion

    #region Initialization

    [SerializeField, ReadOnly] int m_Type = 0;
    [SerializeField] int m_DisappearTime = 3;

    #endregion

    #region State

    [SerializeField, ReadOnly] Rigidbody2D m_Target;
    [SerializeField, ReadOnly] bool m_IsDead;
    [SerializeField, ReadOnly] int m_Health;

    #endregion

    #region Properties

    EnemyData Data => m_EnemyData[m_Type];

    float Speed => m_EnemyData[m_Type].Speed;

    int Health
    {
        get => m_Health;
        set
        {
            if (m_IsDead) return;
            var damage = m_Health - value;
            if (damage > 0)
            {
                ShowDamage(damage);
            }
            
            if(value > MaxHealth)
                m_Health = MaxHealth;
            else if (value <= 0)
            {
                m_Health = 0;
                m_IsDead = true;
                Dead();
                AudioManager.Get().PlaySfx(AudioManager.Sfx.Dead);
                GetReward();
            }
            else
                m_Health = value;
        }
    }
    int MaxHealth => m_EnemyData[m_Type].MaxHealth;
    SpriteLibraryAsset GetSpriteLibraryAsset() => m_EnemyData[m_Type].GetSpriteLibraryAsset();

    #endregion

    #region Buffer

    // FixedUpdate
    Vector2 position;
    Vector2 dir;
    
    // KnockBack
    WaitForFixedUpdate waitForFixedUpdate;
    Vector3 knockBackDir;
    
    // Dead
    Gold gold;
    WaitForSeconds disappearTime;
    
    // Damage
    Vector3 hitPosition;

    #endregion

    #region Event Functions

    // Survival Game Manager
    void OnStageClear_Event(int _nextStage)
    {
        Dead();
    }

    void OnGameClear_Event()
    {
        Dead();
    }

    void OnGameOver_Event()
    {
        m_Rigidbody.simulated = false;
    }

    #endregion

    #region Monobehaviour

    void FixedUpdate()
    {
        // 게임 정지
        if (m_TimeManager.IsPaused) return;

        if (m_IsDead || m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Hit")) return;
        
        position = m_Rigidbody.position;
        dir = m_Target.position - position;
        Vector2 next = Speed * Time.fixedDeltaTime * dir.normalized;
        m_Rigidbody.MovePosition(position + next);
        m_Rigidbody.velocity = Vector2.zero;
    }

    void LateUpdate()
    {
        m_SpriteRenderer.flipX = dir.x < 0;
    }

    void OnTriggerEnter2D(Collider2D _other)
    {
        if (m_IsDead) return;
        if (!_other.CompareTag("Bullet")) return;
        
        hitPosition = _other.bounds.ClosestPoint(transform.position);

        Health -= _other.GetComponent<Bullet>().Damage;
        m_Animator.SetTrigger("Hit");
        StartCoroutine(KnockBack());

        AudioManager.Get().PlaySfx(AudioManager.Sfx.Hit);
    }

    #endregion

    #region Actor

    protected override void AssignComponents()
    {
        base.AssignComponents();
        
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_SpriteLibrary = GetComponent<SpriteLibrary>();
        m_Animator = GetComponent<Animator>();
        m_Collider = GetComponent<Collider2D>();
    }

    protected override void BindEventFunctions()
    {
        base.BindEventFunctions();

        // Time Manager 이벤트 바인딩
        if (m_TimeManager)
        {
            m_TimeManager.OnPause += OnPause_Event;
            m_TimeManager.OnResume += OnResume_Event;
        }
        
        // SurvivalGameManager 이벤트 바인딩
        SurvivalGameManager survivalGameManager = SurvivalGameManager.Get();
        if (survivalGameManager)
        {
            survivalGameManager.OnGameClear += OnGameClear_Event;
            survivalGameManager.OnStageClear += OnStageClear_Event;
            survivalGameManager.OnGameOver += OnGameOver_Event;
        }
    }

    protected override void UnbindEventFunctions()
    {
        base.UnbindEventFunctions();

        // Time Manager 이벤트 언바인딩
        if (m_TimeManager)
        {
            m_TimeManager.OnPause -= OnPause_Event;
            m_TimeManager.OnResume -= OnResume_Event;
        }
        
        // SurvivalGameManager 이벤트 언바인딩
        SurvivalGameManager survivalGameManager = SurvivalGameManager.Get();
        if (survivalGameManager)
        {
            survivalGameManager.OnGameClear -= OnGameClear_Event;
            survivalGameManager.OnStageClear -= OnStageClear_Event;
            survivalGameManager.OnGameOver -= OnGameOver_Event;
        }
    }

    protected override void Awake_Event()
    {
        base.Awake_Event();
        
        // 컴포넌트 할당
        m_TimeManager = GlobalGameManager.Instance.GetTimeManager();
        
        // 변수 할당
        waitForFixedUpdate = new WaitForFixedUpdate();
        disappearTime = new WaitForSeconds(m_DisappearTime);
    }

    protected override void OnEnable_Event()
    {
        base.OnEnable_Event();
        
        // 부활
        Revive();
        
        m_SpriteLibrary.spriteLibraryAsset = GetSpriteLibraryAsset();

        // 목표물을 플레이어로 설정
        if(SurvivalGameManager.Get())
            m_Target = SurvivalGameManager.Get().GetPlayer().GetComponent<Rigidbody2D>();
    }

    #endregion

    #region Method

    void ShowDamage(float _damage)
    {
        if (m_FloatingTextDamage is null) return;

        var floatingTextDamage = PoolManager.GetInstance<DamageUI>(m_FloatingTextDamage);
        floatingTextDamage.Init(_damage, hitPosition);
        floatingTextDamage.gameObject.SetActive(true);
    }

    IEnumerator KnockBack()
    {
        yield return waitForFixedUpdate;
        knockBackDir = (transform.position - SurvivalGameManager.Get().GetPlayer().transform.position).normalized;
        m_Rigidbody.AddForce(knockBackDir * 3, ForceMode2D.Impulse);
    }

    void Dead()
    {
        m_IsDead = true;
        m_Animator.SetBool("Dead", m_IsDead);
        m_Collider.enabled = false;
        m_Rigidbody.simulated = false;
        m_SpriteRenderer.sortingLayerName = "Dead";

        // 일정 시간 뒤 시체가 사라짐
        StartCoroutine(Disappear());
    }

    void GetReward()
    {
        // 경험치 획득
        SurvivalGameManager.Get().GetExp(Data.Exp);

        // 일정 확률로 골드 드랍
        if (m_GoldPrefab)
        {
            if (Random.Range(0, 1f) <= Data.DropProbability)
            {
                gold = PoolManager.GetInstance<Gold>(m_GoldPrefab);
                gold.transform.position = transform.position;
                gold.Init(Data.Gold);
                gold.gameObject.SetActive(true);
            }
        }
    }

    IEnumerator Disappear()
    {
        yield return disappearTime;
        PoolManager.ReleaseInstance(gameObject);
    }

    void Revive()
    {
        // Dead의 반대 동작
        m_IsDead = false;
        m_Animator.SetBool("Dead", m_IsDead);
        m_Collider.enabled = true;
        m_Rigidbody.simulated = true;
        m_SpriteRenderer.sortingLayerName = "Enemy";

        // 체력 설정
        m_Health = MaxHealth;
    }

    #endregion

    #region API

    public void Init(SpawnData _spawnData)
    {
        m_Type = Mathf.Min(_spawnData.Type, m_EnemyData.Length - 1);
    }

    #endregion
}
