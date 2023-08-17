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

    PlayerStats m_PlayerStats;

    void Awake()
    {
        m_PlayerStats = PlayerState.Get().GetStatsComponent();
    }

    void OnUpdate_Event()
    {
        var totalStats = m_PlayerStats.TotalStats;
        m_TotalAttackText.text = totalStats.Attack.ToString();
        m_TotalMaxHealthText.text = totalStats.MaxHealth.ToString();
    }

    void OnEnable()
    {
        PlayerState.Get().GetStatsComponent().OnUpdate += OnUpdate_Event;
        OnUpdate_Event();
    }

    void OnDisable()
    {
        PlayerState.Get().GetStatsComponent().OnUpdate -= OnUpdate_Event;
    }

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
