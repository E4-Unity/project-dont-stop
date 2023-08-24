using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDBoss : MonoBehaviour
{
    public enum InfoType
    {
        BossHP,
        EliteHP,
        Time,
        Health
    }

    public InfoType m_Type;

    Text m_Text;
    Slider m_Slider;

    void Awake()
    {
        m_Text = GetComponent<Text>();
        m_Slider = GetComponent<Slider>();
    }

    void LateUpdate()
    {
        switch (m_Type)
        {
            case InfoType.BossHP:
                if (GameManagerBoss.Get().portalTouch == true)
                {
                    m_Slider.value = GameManagerBoss.Get().BossHP / (float)GameManagerBoss.Get().MaxBossHP;
                }
                break;
            case InfoType.Time:
                float remainTime = GameManagerBoss.Get().MaxPlayTime - GameManagerBoss.Get().PlayTime;
                int min = Mathf.FloorToInt(remainTime / 60);
                int sec = Mathf.FloorToInt(remainTime % 60);
                m_Text.text = $"{min:D2}:{sec:D2}";
                break;
            case InfoType.Health:
                m_Slider.value = GameManagerBoss.Get().Health / (float)GameManagerBoss.Get().MaxHealth;
                break;
        }
    }
}
