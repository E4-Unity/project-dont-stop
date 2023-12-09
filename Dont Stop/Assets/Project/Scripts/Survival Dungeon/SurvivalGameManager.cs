using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class SurvivalGameManager : GameManger
{
    /* 컴포넌트 */
    TimeManager m_TimeManager;
    
    #region Static

    public new static SurvivalGameManager Get() => (SurvivalGameManager)GameManger.Instance;

    #endregion
    
    #region Reference

    [Header("# Game Object")]
    [SerializeField] Player m_Player;
    [SerializeField] PoolManager m_PoolManager;
    [SerializeField] LevelUp m_LevelUp_UI;

    public Player GetPlayer() => m_Player;
    public PoolManager GetPoolManager() => m_PoolManager;
    protected LevelUp GetLevelUp() => m_LevelUp_UI;
    
    #endregion



    #region Field

    [Header("# Player State")] 
    [SerializeField] int m_PlayerID;
    [SerializeField] float m_MaxHealth = 100;
    [SerializeField, ReadOnly] float m_Health;
    [SerializeField, ReadOnly] int m_Level;
    [SerializeField, ReadOnly] int m_Kill;
    [SerializeField, ReadOnly] int m_TotalKill;
    [SerializeField, ReadOnly] int m_Exp;
    [SerializeField, ReadOnly] int m_TotalExp; 
    [SerializeField] int[] m_NextExp = { 3, 5, 10, 100, 150, 210, 280, 360, 450, 600 };
    
    // 스테이지
    [FormerlySerializedAs("m_StageTime")] [SerializeField] int m_MaxStageTime = 10; // 스테이지 별 최대 시간
    [FormerlySerializedAs("m_MaxStage")] [SerializeField] int m_MaxStageCount = 10; // 최대 스테이지
    [SerializeField] int m_StageCount = 0; // 다음 스테이지
    [SerializeField] int[] m_MaxKill = { 10, 20 };
    [SerializeField] int m_StageWaitTIme = 2;

    #endregion

    
    #region Property
    public int PlayerID => m_PlayerID;
    public float Health
    {
        get => m_Health;
        set => m_Health = value;
    }
    public float MaxHealth => m_MaxHealth;
    public int Level => m_Level;

    public int Kill => m_Kill;
    public int TotalKill => m_TotalKill;
    public int CurrentStage => m_StageCount - 1;

    public int MaxKill => m_MaxKill[Mathf.Min(CurrentStage, m_MaxKill.Length - 1)];
    public int Exp => m_Exp;
    public int NextExp => m_NextExp[Mathf.Min(m_Level, m_NextExp.Length - 1)];

    #endregion // 스테이지 클리어를 위한 목표 킬 수

    #region Event
    
    public event Action<int> OnStageStart;
    public event Action<int> OnStageClear;

    #endregion

    #region Event Function
    
    void BindEventFunctions()
    {
        m_TimeManager.OnTimerFinish += GameOver;
    }

    #region GameManager
    
    protected override void OnGameStart_Event()
    {
        base.OnGameStart_Event();
        m_Health = m_MaxHealth;

        m_Player.gameObject.SetActive(true);
        
        //TODO 리팩토링 필요
        // 선택한 캐릭터 무기 사용
        if(SurvivalGameState.Instance.GetEquipmentComponent().WeaponData.WeaponType == EWeaponType.Melee)
            m_LevelUp_UI.Select(0);
        else
            m_LevelUp_UI.Select(1);
        
        AudioManager.Get().PlayBgm(true);
        
        StageStart();
    }

    protected override void OnGameOver_Event()
    {
        base.OnGameOver_Event();
        AudioManager.Get().PlaySfx(AudioManager.Sfx.Lose);
    }

    protected override void OnGameClear_Event()
    { 
        base.OnGameClear_Event();
        AudioManager.Get().PlaySfx(AudioManager.Sfx.Win);
    }

    protected override void OnGameEnd_Event()
    {
        base.OnGameEnd_Event();
        AudioManager.Get().PlayBgm(false);
        SurvivalGameState.Instance.Exp = m_TotalExp;

        StartCoroutine(DelayGameFinish());
    }

    protected override void OnGameFinish_Event()
    {
        base.OnGameFinish_Event();
        m_TimeManager.PauseGame();
    }

    protected override void OnGameExit_Event()
    {
        base.OnGameExit_Event();
        Destroy(SurvivalGameState.Instance);
    }

    #endregion

    #endregion

    #region Method

    void StageStart()
    {
        OnStageStart?.Invoke(m_StageCount);
        m_TimeManager.StartGame(m_MaxStageTime, m_StageWaitTIme);
        m_StageCount++;
    }

    void StageClear()
    {
        // 모든 스테이지 클리어 시, 게임 클리어
        if (m_StageCount == m_MaxStageCount)
        {
            GameClear();
            return;
        }
        
        // 스테이지 클리어 이벤트
        AudioManager.Get().PlaySfx(AudioManager.Sfx.LevelUp);
        OnStageClear?.Invoke(CurrentStage);
        
        // 다음 스테이지 시작
        StageStart();
    }

    #endregion

    #region Coroutine
    
    IEnumerator DelayGameFinish(float _delayTime = 0.5f)
    {
        yield return new WaitForSeconds(_delayTime);

        GameFinish();
    }

    #endregion

    #region API

    // TODO 몬스터 별로 경험치 다르게 설정 예정
    public void GetExp(int _exp)
    {
        m_Exp += _exp;
        m_TotalExp += _exp;
        GetKill();

        // TODO 한 번에 2레벨 이상 오르는 경우 고려해야함
        if (m_Exp >= NextExp)
        {
            m_Exp -= NextExp;
            m_Level++;
            m_LevelUp_UI.Show();
        }
    }

    public void GetKill()
    {
        m_Kill++;
        m_TotalKill++;
        
        // 스테이지 목표 킬 수 달성 시
        if (m_Kill == MaxKill)
        {
            StageClear();
            m_Kill = 0;
        }
    }

    #endregion

    #region Monobehaviour

    protected override void Awake()
    {
        base.Awake();
        
        // 컴포넌트 할당
        m_TimeManager = GlobalGameManager.Instance.GetTimeManager();
        
        // 타깃 프레임 설정
        Application.targetFrameRate = 60;
    }

    void OnEnable()
    {
        BindEventFunctions();
    }

    void Start()
    {
        GameStart();
    }

    #endregion

}
