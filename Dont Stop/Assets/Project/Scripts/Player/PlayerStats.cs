using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    /* 컴포넌트 */
    PlayerState m_PlayerState;
    UPlayerData m_PlayerData;
    UCharacterData m_CharacterData;
    PlayerEquipment m_EquipmentComponent;
    
    /* 필드 */
    UBasicAttribute m_TotalStats;

    /* 프로퍼티 */
    public UBasicAttribute TotalStats => m_TotalStats;

    /* 이벤트 */
    public event Action OnUpdate;

    /* API */
    public void Init(PlayerEquipment _equipmentComponent)
    {
        m_PlayerState = GlobalGameManager.Instance.GetPlayerState();
        m_PlayerData = m_PlayerState.PlayerData;
        m_CharacterData = m_PlayerState.CharacterData;
        m_EquipmentComponent = _equipmentComponent;

        m_PlayerState.OnCharacterDataUpdate += _data =>
        {
            m_CharacterData = _data;
            CalculatePlayerStats();
        };
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
        m_TotalStats += m_PlayerData.Attribute;
        
        // 캐릭터 스탯 반영
        m_TotalStats += m_CharacterData.Attribute;
        
        // 장비 스탯 반영
        foreach (var gearSlot in m_EquipmentComponent.GearSlots)
        {
            m_TotalStats += gearSlot.Value.Attribute;
        }
        
        OnUpdate?.Invoke();
    }
}
