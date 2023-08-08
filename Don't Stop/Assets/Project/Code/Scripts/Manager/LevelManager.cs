using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour, IManager
{
    public static LevelManager Instnace;
    public static LevelManager Get() => Instnace;

    [Header("Reference")]
    [SerializeField] Image m_FadeImage;
    
    [Header("Initialization")]
    [SerializeField] float m_RefreshTime = 0.1f;
    [SerializeField] float m_BufferingTime = 1f;
    [SerializeField] float m_FadeTime = 1f;
    public event Action<float> OnRefresh;
    public Action<bool> OnFinish;
    
    /* 버퍼 */
    AsyncOperation operation;
    WaitForSecondsRealtime fadeTime;
    WaitForSecondsRealtime refreshTime;
    WaitForSecondsRealtime bufferingTime;
    
    /* 이벤트 함수 */
    void OnFinish_Event(bool _enableBuffering)
    {
        StartCoroutine(FinishLoading(_enableBuffering));
    }

    void OnSceneLoaded_Event(Scene _loadedScene, LoadSceneMode _loadSceneMode)
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        m_FadeImage.CrossFadeAlpha(0, m_FadeTime, true);
        yield return fadeTime;
        m_FadeImage.enabled = false;
    }

    /* 메서드 */
    IEnumerator Loading(string _nextSceneName)
    {
        operation = SceneManager.LoadSceneAsync(_nextSceneName);
        
        // 자동 씬 전환 방지
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            OnRefresh?.Invoke(operation.progress / 0.9f); // progress의 최대값은 0.9f
            
            yield return refreshTime;
        }
    }
    
    IEnumerator Loading(int _nextSceneIndex)
    {
        operation = SceneManager.LoadSceneAsync(_nextSceneIndex);
        
        // 자동 씬 전환 방지
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            OnRefresh?.Invoke(operation.progress / 0.9f); // progress의 최대값은 0.9f
            
            yield return refreshTime;
        }
    }

    IEnumerator FinishLoading(bool _enableBuffering)
    {
        /* 시각적인 효과를 위한 고정 로딩 시간 */
        if(_enableBuffering)
            yield return new WaitForSeconds(m_BufferingTime);
        
        // Fade Out 시작
        m_FadeImage.enabled = true;

        m_FadeImage.CrossFadeAlpha(1, m_FadeTime, true);
        yield return fadeTime;
        
        // Fade Out이 끝나면 씬 전환
        operation.allowSceneActivation = true;
    }

    /* API */
    public void ChangeScene(string _nextSceneName)
    {
        StartCoroutine(Loading(_nextSceneName));
    }
    
    public void ChangeScene(int _nextSceneIndex)
    {
        StartCoroutine(Loading(_nextSceneIndex));
    }
    
    /* MonoBehaviour */
    void Awake()
    {
        if (Instnace is null)
        {
            Instnace = this;
            DontDestroyOnLoad(this);

            refreshTime = new WaitForSecondsRealtime(m_RefreshTime);
            fadeTime = new WaitForSecondsRealtime(m_FadeTime);
            bufferingTime = new WaitForSecondsRealtime(m_BufferingTime);
            OnFinish += OnFinish_Event;
            SceneManager.sceneLoaded += OnSceneLoaded_Event;
        }
        else
            Destroy(gameObject);
    }

    public void InitManager()
    {
        
    }
}
