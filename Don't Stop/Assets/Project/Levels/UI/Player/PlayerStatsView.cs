using System;
using System.Collections;
using System.Collections.Generic;
using Presenter;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsView : MonoBehaviour, IPlayerStats
{
    // 플레이어 스탯
    [SerializeField] Text m_TotalAttackText;
    [SerializeField] Text m_TotalMaxHealthText;

    #region IPlayerStats

    public int MaxHealth
    {
        set => m_TotalMaxHealthText.text = value.ToString();
    }

    public int Attack
    {
        set => m_TotalAttackText.text = value.ToString();
    }

    #endregion
}
