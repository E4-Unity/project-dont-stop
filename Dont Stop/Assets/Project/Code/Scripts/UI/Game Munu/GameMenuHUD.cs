using UnityEngine;
using UnityEngine.UI;

public class GameMenuHUD : PollingUI
{
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

    protected override void Refresh()
    {
        if (PlayerState.Get() is null) return;
        // 인벤토리
        gold = PlayerState.Get().GetInventoryComponent().Gold;
        m_Gold.text = gold >= 1000 ? $"{gold / 1000f:F1}K" : gold.ToString();
        
        crystal = PlayerState.Get().GetInventoryComponent().Crystal;
        m_Crystal.text = crystal >= 1000 ? $"{crystal / 1000f:F1}K" : crystal.ToString();
        
        // 플레이어 정보
        PlayerData = PlayerState.Get().PlayerData;
        m_PlayerName.text = PlayerData.PlayerName;
        m_PlayerLevel.text = PlayerData.Level.ToString();
        m_PlayerExpRatio.value = PlayerData.Exp / (float)PlayerData.NextExp;
    }
}
