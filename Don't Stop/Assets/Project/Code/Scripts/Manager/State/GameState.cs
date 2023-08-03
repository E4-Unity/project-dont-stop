using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : GenericMonoSingleton<GameState>
{
    #region Components

    PlayerEquipment m_EquipmentComponent;
    PlayerStats m_StatsComponent;

    PlayerEquipment GetEquipmentComponent() => m_EquipmentComponent;
    PlayerStats GetStatsComponent() => m_StatsComponent;

    #endregion

    #region Properties

    [SerializeField] int m_Gold;

    public int Gold
    {
        get => m_Gold;
        set => m_Gold = value;
    }
    
    [SerializeField] int m_Exp;

    public int Exp
    {
        get => m_Exp;
        set => m_Exp = value;
    }
    
    #endregion

    #region Method

    void CopyPlayerState()
    {
        var playerEquipment = PlayerState.Get().GetEquipmentComponent();
        /* Equipment Component Deep Copy */
        FSavedAttributeData saveData;
        
        // Weapon
        UWeaponData weaponCopy = new UWeaponData();
        playerEquipment.WeaponData.GetSaveData(out saveData);
        weaponCopy.Init(saveData);
        m_EquipmentComponent.WeaponData = weaponCopy;

        // Gears
        foreach (var gearSlot in playerEquipment.GearSlots)
        {
            UGearData gearCopy = new UGearData();
            gearSlot.Value.GetSaveData(out saveData);
            gearCopy.Init(saveData);
            m_EquipmentComponent.GearSlots.Add(gearSlot.Key, gearCopy);
            m_EquipmentComponent.GearDataList.Add(gearCopy);
        }
    }

    void Init()
    {
        CopyPlayerState();
        GetStatsComponent().Init(GetEquipmentComponent().GearSlots);
    }

    #endregion

    #region API

    // TODO GameManager GameOver 이벤트 바인딩?
    public void UpdateResult()
    {
        PlayerState.Get().GetInventoryComponent().Gold += Gold;
    }

    #endregion

    #region Monobehaviour

    protected override void Awake_Implementation()
    {
        base.Awake_Implementation();
        m_EquipmentComponent = GetComponent<PlayerEquipment>();
        m_StatsComponent = GetComponent<PlayerStats>();
    }

    void Start()
    {
        PlayerState.Get().TryInit(Init);
    }

    #endregion
}
