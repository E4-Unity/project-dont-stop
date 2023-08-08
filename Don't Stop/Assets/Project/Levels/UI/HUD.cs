using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    SurvivalGameManager m_GameManager;
    TimeManager m_TimeManager;
    SurvivalGameState m_GameState;

    public enum InfoType
    {
        Exp,
        Level,
        Kill,
        Time,
        Health,
        Stage,
        Gold
    }

    /* State */
    public InfoType m_Type;

    /* Component */
    Text m_Text;
    Slider m_Slider;

    /* MonoBehaviour */
    void Awake()
    {
        m_Text = GetComponent<Text>();
        m_Slider = GetComponent<Slider>();
        m_GameManager = SurvivalGameManager.Get();
        m_TimeManager = TimeManager.Get();
        m_GameState = SurvivalGameState.Get();
    }

    void LateUpdate()
    {
        switch (m_Type)
        {
            case InfoType.Exp:
                m_Slider.value = m_GameManager.Exp / (float)m_GameManager.NextExp;
                break;
            case InfoType.Level:
                m_Text.text = $"Lv.{m_GameManager.Level:F0}";
                break;
            case InfoType.Kill:
                m_Text.text = $"{m_GameManager.TotalKill:F0}";
                break;
            case InfoType.Time:
                var remainTime = m_TimeManager.MaxPlayTime - m_TimeManager.PlayTime;
                int min = Mathf.FloorToInt(remainTime / 60);
                int sec = Mathf.FloorToInt(remainTime % 60);
                m_Text.text = $"{min:D2}:{sec:D2}";
                break;
            case InfoType.Health:
                m_Slider.value = m_GameManager.Health / (float)m_GameManager.MaxHealth;
                break;
            case InfoType.Stage:
                m_Text.text = $"Stage {m_GameManager.CurrentStage + 1:D2}";
                break;
            case InfoType.Gold:
                m_Text.text = $"{m_GameState.Gold}";
                break;
        }
    }
}
