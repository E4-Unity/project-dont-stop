using Presenter;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsView : MonoBehaviour, IPlayerStats
{
    /* 레퍼런스 */
    [Header("UI")]
    [SerializeField] Text m_TotalAttackText;
    [SerializeField] Text m_TotalMaxHealthText;

    /* 컴포넌트 */
    PlayerState m_PlayerState;
    PlayerStats m_PlayerStats;

    /* MonoBehaviour */
    void Awake()
    {
        m_PlayerState = GlobalGameManager.Instance.GetPlayerState();
        m_PlayerStats = m_PlayerState.GetStatsComponent();
    }

    void OnUpdate_Event()
    {
        var totalStats = m_PlayerStats.TotalStats;
        m_TotalAttackText.text = totalStats.Attack.ToString();
        m_TotalMaxHealthText.text = totalStats.MaxHealth.ToString();
    }

    void OnEnable()
    {
        m_PlayerState.GetStatsComponent().OnUpdate += OnUpdate_Event;
        OnUpdate_Event();
    }

    void OnDisable()
    {
        m_PlayerState.GetStatsComponent().OnUpdate -= OnUpdate_Event;
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
