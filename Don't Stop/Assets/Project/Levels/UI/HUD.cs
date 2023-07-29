using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType
    {
        Exp,
        Level,
        Kill,
        Time,
        Health
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
    }

    void LateUpdate()
    {
        switch (m_Type)
        {
            case InfoType.Exp:
                m_Slider.value = GameManager.Get().Exp / (float)GameManager.Get().NextExp;
                break;
            case InfoType.Level:
                m_Text.text = $"Lv.{GameManager.Get().Level:F0}";
                break;
            case InfoType.Kill:
                m_Text.text = $"{GameManager.Get().Kill:F0}";
                break;
            case InfoType.Time:
                var remainTime = GameManager.Get().MaxPlayTime - GameManager.Get().PlayTime;
                int min = Mathf.FloorToInt(remainTime / 60);
                int sec = Mathf.FloorToInt(remainTime % 60);
                m_Text.text = $"{min:D2}:{sec:D2}";
                break;
            case InfoType.Health:
                m_Slider.value = GameManager.Get().Health / (float)GameManager.Get().MaxHealth;
                break;
        }
    }
}
