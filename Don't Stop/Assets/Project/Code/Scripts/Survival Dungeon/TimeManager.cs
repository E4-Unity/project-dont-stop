using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeManager : GenericMonoSingleton<TimeManager>
{
    #region State

    [SerializeField, ReadOnly] bool m_IsPaused = false;
    [SerializeField, ReadOnly] float m_TotalPlayTime;
    [SerializeField, ReadOnly] float m_PlayTime;
    [SerializeField] float m_MaxPlayTime = 20f;

    public bool IsPaused => m_IsPaused;
    public float TotalPlayTime => m_TotalPlayTime;
    public float PlayTime => m_PlayTime;
    public float MaxPlayTime => m_MaxPlayTime;
    
    #endregion

    #region Coroutine

    IEnumerator m_PlayTimer;

    #endregion

    #region Event

    public event Action OnPause;
    public event Action OnResume;
    public event Action OnTimerReset;
    public event Action OnTimerStart;
    public event Action OnTimerFinish;

    #endregion

    #region Method

    IEnumerator PlayTimer(int _waitTime)
    {
        yield return new WaitForSecondsRealtime(_waitTime);
        OnTimerStart?.Invoke();
        
        while (true)
        {
            yield return null;
            m_TotalPlayTime += Time.deltaTime;
            m_PlayTime += Time.deltaTime;

            if (m_PlayTime > m_MaxPlayTime)
            {
                m_PlayTime = m_MaxPlayTime;
                OnTimerFinish?.Invoke();
            }
        }
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    #endregion

    #region API

    public void StartGame(int _maxPlayTime, int _waitTime)
    {
        // 기존 타이머 제거
        if(m_PlayTimer is not null)
            StopCoroutine(m_PlayTimer);
        
        // 타이머 리셋
        m_PlayTime = 0;
        m_MaxPlayTime = _maxPlayTime;
        OnTimerReset?.Invoke();

        // 새로운 타이머 실행
        m_PlayTimer = PlayTimer(_waitTime);
        StartCoroutine(m_PlayTimer);
    }

    public void PauseGame()
    {
        m_IsPaused = true;
        Time.timeScale = 0;
        OnPause?.Invoke();
        StopCoroutine(m_PlayTimer);
    }

    public void ResumeGame()
    {
        m_IsPaused = false;
        Time.timeScale = 1;
        OnResume?.Invoke();
        StartCoroutine(m_PlayTimer);
    }

    #endregion

    #region Monobehaviour

    protected override void OnDestroy()
    {
        base.OnDestroy();
        Time.timeScale = 1;
    }

    #endregion
}
