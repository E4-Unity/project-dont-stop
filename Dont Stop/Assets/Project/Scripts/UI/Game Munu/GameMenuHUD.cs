using UnityEngine;
using UnityEngine.UI;

public class GameMenuHUD : PollingUI
{
    /* 컴포넌트 */
    PlayerState m_PlayerState;
    PlayerInventory m_PlayerInventory;
    
    // 인벤토리
    [SerializeField] Text m_Gold;
    [SerializeField] Text m_Crystal;
    
    // 플레이어 정보
    [SerializeField] Text m_PlayerName;
    [SerializeField] Text m_PlayerLevel;
    [SerializeField] Slider m_PlayerExpRatio;

    // 버퍼
    int gold;
    int crystal;
    UPlayerData PlayerData;
    
    /* MonoBehaviour */
    protected override void Awake_Event()
    {
        base.Awake_Event();

        m_PlayerState = GlobalGameManager.Instance.GetPlayerState();
        m_PlayerInventory = m_PlayerState.GetInventoryComponent();
    }

    protected override void Refresh()
    {
        if (m_PlayerState is null) return;
        // 인벤토리
        gold = m_PlayerInventory.Gold;
        m_Gold.text = gold >= 1000 ? $"{gold / 1000f:F1}K" : gold.ToString();
        
        crystal = m_PlayerInventory.Crystal;
        m_Crystal.text = crystal >= 1000 ? $"{crystal / 1000f:F1}K" : crystal.ToString();
        
        // 플레이어 정보
        PlayerData = m_PlayerState.PlayerData;
        m_PlayerName.text = PlayerData.PlayerName;
        m_PlayerLevel.text = PlayerData.Level.ToString();
        m_PlayerExpRatio.value = PlayerData.Exp / (float)PlayerData.NextExp;
    }
}
