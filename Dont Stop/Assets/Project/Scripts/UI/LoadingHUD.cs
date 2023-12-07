using System;
using UnityEngine;
using UnityEngine.UI;

public class LoadingHUD : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] Slider m_Slider;
    [SerializeField] Text m_PercentageText;
    [SerializeField] Text m_StateText;

    /* 이벤트 함수 */
    void OnRefresh_Event(float loadingRate)
    {
        if(m_Slider)
            m_Slider.value = loadingRate;
        if(m_PercentageText)
            m_PercentageText.text = $"{loadingRate * 100:F0} %";

        if (Mathf.Approximately(loadingRate, 1f))
        {
            SceneLoadingManager.Instance.OnProgressRefreshed -= OnRefresh_Event;
            if (m_StateText)
                m_StateText.text = "로딩 완료";
        }
    }

    /* MonoBehaviour 가상 함수 */
    void Awake()
    {
        m_Slider = GetComponent<Slider>();

        OnRefresh_Event(0);
        SceneLoadingManager.Instance.OnProgressRefreshed += OnRefresh_Event;
    }

    void OnDestroy()
    {
        SceneLoadingManager.Instance.OnProgressRefreshed -= OnRefresh_Event;
    }
}
