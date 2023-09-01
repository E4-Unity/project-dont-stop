using System.Collections;
using System.Collections.Generic;
using Presenter;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDataView : MonoBehaviour, IPlayerData
{
    // 플레이어 정보
    [SerializeField] Text m_PlayerNameText;
    [SerializeField] Text m_PlayerLevelText;
    [SerializeField] Slider m_PlayerExpRatio;

    /* 버퍼 */
    int exp;
    int nextExp;

    public string PlayerName
    {
        set => m_PlayerNameText.text = value;
    }

    public int PlayerLevel
    {
        set => m_PlayerLevelText.text = value.ToString();
    }

    // TODO SetExpRatio의 중복 호출을 막는 방법?
    public int PlayerExp
    {
        set
        {
            exp = value;
            SetExpRatio();
        }
    }

    public int PlayerNextExp
    {
        set
        {
            nextExp = value;
            SetExpRatio();
        }
    }

    void SetExpRatio()
    {
        m_PlayerExpRatio.value = exp / (float)nextExp;
    }
}