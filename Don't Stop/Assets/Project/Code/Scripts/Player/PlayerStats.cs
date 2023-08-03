using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    #region Reference

    [SerializeField] UPlayerData m_PlayerData;
    [SerializeField] UCharacterData m_CharacterData;
    Dictionary<EGearType, UGearData> m_GearSlots;

    #endregion

    #region State
    
    [SerializeField] UBasicAttribute m_TotalStats;

    public UBasicAttribute TotalStats => m_TotalStats;

    #endregion

    #region API

    public void Init(Dictionary<EGearType, UGearData> _gearSlots)
    {
        m_PlayerData = PlayerState.Get().PlayerData;
        m_CharacterData = PlayerState.Get().CharacterData;
        m_GearSlots = _gearSlots;

        CalculatePlayerStats();
    }
    
    // TODO 이벤트 바인딩으로 변경
    public void CalculatePlayerStats()
    {
        m_TotalStats = new UBasicAttribute();
        
        // 플레이어 스탯 반영
        m_TotalStats += m_PlayerData.GetAttribute();
        
        // 캐릭터 스탯 반영
        m_TotalStats += m_CharacterData.GetAttribute();
        
        // 장비 스탯 반영
        foreach (var gearSlot in m_GearSlots)
        {
            m_TotalStats += gearSlot.Value.GetAttribute();
        }
    }

    #endregion

}
