using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Build Settings 에 설정된 Index 매핑
public enum EBuildScene
{
    None = -1,
    Title = 0,
    Lobby = 1,
    Survival = 2
}

public class SceneLoadingManager : E4.Utility.GenericMonoSingleton<SceneLoadingManager>
{
    // 화면 전환 연출 타입
    public enum ETransitionType
    {
        None,
        Fade,
        Animation
    }
    
    /* 정적 필드 */
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void GenerateLevelManagerPrefabInstance()
    {
        var prefab = Resources.Load<GameObject>("Level Manager");
        if (prefab is not null)
        {
            Instantiate(prefab);
        }
    }

    /* 필드 */
    [Header("페이드 인 / 아웃")]
    [SerializeField] Image m_FadeImage;
    [SerializeField] float m_FadeTime = 1f;

    [Header("기타")] 
    [SerializeField] Image m_DisableImage; // 입력을 막기 위한 이미지
    [SerializeField] float m_RefreshTime = 0.02f; // 로딩 진행률 갱신 주기
    
    ETransitionType m_TransitionType;
    
    /* 이벤트 */
    public event Action<float> OnProgressRefreshed; // 로딩 게이지 바 UI 갱신을 위한 이벤트
    public event Action OnSceneShown;
    
    AsyncOperation operation;
    WaitForSecondsRealtime waitForFadeTime;
    WaitForSecondsRealtime refreshTime;
    
    /* GenericMonoSingleton */
    protected override void Init()
    {
        base.Init();
        
        // 모든 씬에서 사용
        DontDestroyOnLoad(gameObject);
        
        // 자주 사용하는 변수 초기화
        refreshTime = new WaitForSecondsRealtime(m_RefreshTime);
        waitForFadeTime = new WaitForSecondsRealtime(m_FadeTime);
    }

    /* MonoBehaviour */
    void Start()
    {
        // 처음에는 페이드 아웃 연출로 시작
        m_TransitionType = ETransitionType.Fade;
        m_FadeImage.gameObject.SetActive(true);
        StartCoroutine(ShowScreen());
        
        // 씬 전환 이후 화면 정리를 위한 이벤트 바인딩
        SceneManager.sceneLoaded += OnSceneLoaded_Event;
    }

    /* API */
    public void ChangeScene(EBuildScene target, ETransitionType transitionType = ETransitionType.Fade) =>
        ChangeScene((int)target, transitionType);
    
    public void ChangeScene(int target, ETransitionType transitionType = ETransitionType.Fade)
    {
        // 화면 전환 연출 방법 설정
        m_TransitionType = transitionType;
        
        // 씬 전환 시퀀스 실행
        m_DisableImage.gameObject.SetActive(true);
        StartCoroutine(Loading(target));
    }

    /* 이벤트 함수 */
    void OnSceneLoaded_Event(Scene scene, LoadSceneMode loadSceneMode)
    {
        StartCoroutine(ShowScreen());
    }

    /* 메서드 */
    IEnumerator Loading(int target)
    {
        // 사용자 UI 입력 금지
        m_DisableImage.gameObject.SetActive(true);
        
        // 비동기 씬 로딩 시작
        operation = SceneManager.LoadSceneAsync(target);
        
        // 자동 씬 전환 금지
        operation.allowSceneActivation = false;

        // 로딩 진행률 갱신
        while (!Mathf.Approximately(operation.progress, 0.9f))
        {
            OnProgressRefreshed?.Invoke(operation.progress / 0.9f); // progress의 최대값은 0.9f
            
            yield return refreshTime;
        }
        
        OnProgressRefreshed?.Invoke(1);

        // 화면 가리기
        yield return StartCoroutine(HideScreen());
        
        // 자동 씬 전환 허용
        operation.allowSceneActivation = true;
    }
    
    IEnumerator HideScreen()
    {
        // 선택된 화면 전환 연출 진행
        switch (m_TransitionType)
        {
            case ETransitionType.Fade:
                // 페이드 이미지 활성화
                m_FadeImage.gameObject.SetActive(true);
                
                // 페이드 인
                yield return StartCoroutine(FadeIn());
                break;
            default:
                break;
        }
    }
    
    IEnumerator ShowScreen()
    {
        // 선택된 화면 전환 연출 진행
        switch (m_TransitionType)
        {
            case ETransitionType.Fade:
                // 페이드 아웃
                yield return StartCoroutine(FadeOut());
                
                // 페이드 이미지 비활성화
                m_FadeImage.gameObject.SetActive(false);
                break;
            default:
                break;
        }
        
        // 사용자 UI 입력 허용
        m_DisableImage.gameObject.SetActive(false);
        
        // 이벤트 호출
        OnSceneShown?.Invoke();
    }
    
    IEnumerator FadeIn()
    {
        m_FadeImage.canvasRenderer.SetAlpha(0);
        return Fade(1);
    }

    IEnumerator FadeOut()
    {
        m_FadeImage.canvasRenderer.SetAlpha(1);
        return Fade(0);
    }

    IEnumerator Fade(float alpha)
    {
        m_FadeImage.CrossFadeAlpha(alpha, m_FadeTime, true);
        yield return waitForFadeTime;
    }
}
