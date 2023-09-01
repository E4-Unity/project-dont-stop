using System;

public abstract class GameManger : GenericMonoSingleton<GameManger>
{
    #region Field

    bool m_IsGameEnd;
    public bool IsGameEnd => m_IsGameEnd;

    #endregion
    
    #region Event

    // 씬 로딩 후 초기화 단계
    public event Action OnGameEnter;
    
    // 게임 시작
    public event Action OnGameStart;
    
    // 게임 클리어 성공
    public event Action OnGameClear;

    // 게임 클리어 실패
    public event Action OnGameOver;

    // 게임 종료
    public event Action OnGameEnd;
    
    // 게임이 완전히 종료됨 (결과 표시 등의 작업)
    public event Action OnGameFinish;
    
    // 다음 씬을 로딩하기 이전 단계
    public event Action OnGameExit;

    #endregion

    #region Event Functions

    protected virtual void OnGameEnter_Event() {  }
    
    protected virtual void OnGameStart_Event() {  }
    
    protected virtual void OnGameClear_Event() {  }
    
    protected virtual void OnGameOver_Event() {  }
    
    protected virtual void OnGameEnd_Event() {  }
    
    protected virtual void OnGameFinish_Event() { }
    
    protected virtual void OnGameExit_Event() {  }

    #endregion

    #region Method

    protected abstract void FindAllSingletons();

    #endregion

    #region API

    public void GameEnter()
    {
        OnGameEnter?.Invoke();
        OnGameEnter_Event();
    }
    
    public void GameStart()
    {
        OnGameStart?.Invoke();
        OnGameStart_Event();
    }

    public void GameClear()
    {
        OnGameClear?.Invoke();
        OnGameClear_Event();
        GameEnd();
    }
    
    public void GameOver()
    {
        OnGameOver?.Invoke();
        OnGameOver_Event();
        GameEnd();
    }
    
    public void GameEnd()
    {
        if (m_IsGameEnd) return;
        m_IsGameEnd = true;
        
        OnGameEnd?.Invoke();
        OnGameEnd_Event();
    }

    public void GameFinish()
    {
        OnGameFinish?.Invoke();
        OnGameFinish_Event();
    }
    
    public void GameExit()
    {
        OnGameExit?.Invoke();
        OnGameExit_Event();
    }

    #endregion

    #region Monobehaviour

    protected override void Awake()
    {
        base.Awake();
        FindAllSingletons();
    }

    #endregion
}
