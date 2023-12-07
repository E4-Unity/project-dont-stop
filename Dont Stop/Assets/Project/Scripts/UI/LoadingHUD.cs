using UnityEngine;
using UnityEngine.UI;

public class LoadingHUD : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] Slider m_Slider;
    [SerializeField] Text m_PercentageText;
    [SerializeField] Text m_StateText;
    
    /* 컴포넌트 */
    SceneLoadingManager sceneLoadingManager;
    
    /* MonoBehaviour */
    void Awake()
    {
        // 컴포넌트 할당
        sceneLoadingManager = GlobalGameManager.Instance.GetSceneLoadingManager();
        m_Slider = GetComponent<Slider>();

        OnRefresh_Event(0);
        sceneLoadingManager.OnProgressRefreshed += OnRefresh_Event;
    }

    void OnDestroy()
    {
        sceneLoadingManager.OnProgressRefreshed -= OnRefresh_Event;
    }
    
    /* 이벤트 함수 */
    void OnRefresh_Event(float loadingRate)
    {
        // 로딩 진행률 게이지 업데이트
        if(m_Slider)
            m_Slider.value = loadingRate;
        
        // 로딩 진행률 텍스트 업데이트
        if(m_PercentageText)
            m_PercentageText.text = $"{loadingRate * 100:F0} %";

        // 로딩 완료 텍스트 업데이트
        if (Mathf.Approximately(loadingRate, 1f))
        {
            if (m_StateText)
                m_StateText.text = "로딩 완료";
        }
    }
}
