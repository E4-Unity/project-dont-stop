using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    #region Reference

    [SerializeField] UPlayerData m_PlayerData;
    [SerializeField] UCharacterData m_CharacterData;
    [SerializeField] PlayerEquipment m_EquipmentComponent;

    #endregion

    #region State
    
    [SerializeField] UBasicAttribute m_TotalStats;

    public UBasicAttribute TotalStats => m_TotalStats;

    #endregion

    #region Event

    public event Action OnUpdate;

    #endregion

    #region API

    public void Init(PlayerEquipment _equipmentComponent)
    {
        m_PlayerData = PlayerState.Get().PlayerData;
        m_CharacterData = PlayerState.Get().CharacterData;
        m_EquipmentComponent = _equipmentComponent;

        m_PlayerData.OnLevelUpdate += _i => CalculatePlayerStats();
        m_CharacterData.OnLevelUpdate += _i => CalculatePlayerStats();
        m_EquipmentComponent.OnGearUpdate += (_type, _data) => CalculatePlayerStats();
        m_EquipmentComponent.OnWeaponUpdate += _data => CalculatePlayerStats();

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
        foreach (var gearSlot in m_EquipmentComponent.GearSlots)
        {
            m_TotalStats += gearSlot.Value.GetAttribute();
        }
        
        OnUpdate?.Invoke();
    }

    #endregion

}
