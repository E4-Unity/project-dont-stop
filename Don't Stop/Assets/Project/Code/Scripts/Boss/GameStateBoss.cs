using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateBoss : GameState<GameStateBoss>
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


    #region API

    // TODO GameManager GameOver 이벤트 바인딩?
    public void UpdateResult()
    {
        PlayerState.Get().GetInventoryComponent().Gold += Gold;
    }

    #endregion

    #region Monobehaviour

    protected override void Awake()
    {
        base.Awake();
        m_EquipmentComponent = GetComponent<PlayerEquipment>();

        m_StatsComponent = GetComponent<PlayerStats>();
        GetStatsComponent().Init(GetEquipmentComponent());
    }

    private void Start()
    {
        CopyPlayerState();
    }

    // GameState 인터페이스
    protected override void CopyPlayerState()
    {
        var playerEquipment = PlayerState.Get().GetEquipmentComponent();
        /* Equipment Component Deep Copy */

        // Weapon
        UWeaponData weaponCopy = new UWeaponData();
        weaponCopy.Init(playerEquipment.WeaponData.GetSaveData());
        m_EquipmentComponent.WeaponData = weaponCopy;

        // Gears
        foreach (var gearSlot in playerEquipment.GearSlots)
        {
            UGearData gearCopy = new UGearData();
            gearCopy.Init(gearSlot.Value.GetSaveData());
            m_EquipmentComponent.GearSlots.Add(gearSlot.Key, gearCopy);
            m_EquipmentComponent.GearDataList.Add(gearCopy);
        }


    }

    protected override void UpdatePlayerState()
    {
        PlayerState.Get().GetInventoryComponent().Gold += Gold;
        PlayerState.Get().PlayerData.Exp += Exp;
    }

    #endregion
}
