using System;
using UnityEngine;
using UnityEngine.UI;

public enum ESceneList
{
    StartScene,
    MainScene,
    SurvivorDungeon,
    AdventureDungeon,
    Bunker
}

public class LoadingHUD : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] Slider m_Slider;
    [SerializeField] Text m_PercentageText;
    [SerializeField] Text m_StateText;
    [SerializeField] ESceneList m_TargetScene;

    /* 이벤트 함수 */
    void OnRefresh_Event(float _loadingRate)
    {
        if(m_Slider)
            m_Slider.value = _loadingRate;
        if(m_PercentageText)
            m_PercentageText.text = $"{_loadingRate * 100:F0} %";

        if (Mathf.Approximately(_loadingRate, 1f))
        {
            LevelManager.Get().OnRefresh -= OnRefresh_Event;
            if (m_StateText)
                m_StateText.text = "로딩 완료";
;           LevelManager.Get().OnFinish.Invoke(true);
        }
    }

    /* MonoBehaviour 가상 함수 */
    void Awake()
    {
        m_Slider = GetComponent<Slider>();
    }

    void Start()
    {
        LevelManager.Get().OnRefresh += OnRefresh_Event;
        LevelManager.Get().ChangeScene((int)m_TargetScene);
    }
}
