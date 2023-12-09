using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviour
{
    /* 필드 */
    [SerializeField, ReadOnly] bool m_IsPaused = false;
    [SerializeField, ReadOnly] float m_TotalPlayTime;
    [SerializeField, ReadOnly] float m_PlayTime;
    [SerializeField] float m_MaxPlayTime = 20f;
    
    IEnumerator m_PlayTimer;

    /* 프로퍼티 */
    public bool IsPaused => m_IsPaused;
    public float TotalPlayTime => m_TotalPlayTime;
    public float PlayTime => m_PlayTime;
    public float MaxPlayTime => m_MaxPlayTime;

    /* 이벤트 */
    public event Action OnPause;
    public event Action OnResume;
    public event Action OnTimerReset;
    public event Action OnTimerStart;
    public event Action OnTimerFinish;

    /* MonoBehaviour */
    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded_Event;
    }
    
    /* 이벤트 함수 */
    void OnSceneLoaded_Event(Scene scene, LoadSceneMode sceneMode)
    {
        // 정지된 상태라면 씬 전환 시 정지 해제
        if (!m_IsPaused) return;
        
        m_IsPaused = false;
        Time.timeScale = 1;
    }

    /* API */
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
    
    /* 메서드 */
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
}
