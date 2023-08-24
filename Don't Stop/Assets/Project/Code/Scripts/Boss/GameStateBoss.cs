using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateBoss : GameState<GameStateBoss>
{
    PlayerEquipment m_EquipmentComponent;
    PlayerStats m_StatsComponent;
    EquipmentComponentBoss m_EquipmentComponentBoss;

    public PlayerEquipment GetEquipmentComponent() => m_EquipmentComponent;
    public EquipmentComponentBoss GetEquipmentComponentBoss() => m_EquipmentComponentBoss;
    public PlayerStats GetStatsComponent() => m_StatsComponent;

    protected override void Awake()
    {
        base.Awake();
        m_EquipmentComponent = GetComponent<PlayerEquipment>();

        m_StatsComponent = GetComponent<PlayerStats>();
        GetStatsComponent().Init(GetEquipmentComponent());

        m_EquipmentComponentBoss = GetComponent<EquipmentComponentBoss>();
        m_EquipmentComponentBoss.Init();
    }

    private void Start()
    {
        CopyPlayerState();
    }

    // GameState 인터페이스
    protected override void CopyPlayerState()
    {
        var playerEquipment = PlayerState.Get().GetEquipmentComponent();

        // Weapon
        var weaponData = playerEquipment.WeaponData;
        UWeaponData weaponCopy = new UWeaponData();
        weaponCopy.Init(playerEquipment.WeaponData.GetSaveData());
        m_EquipmentComponent.WeaponData = weaponCopy;

        // Gears
        foreach (var gearSlot in playerEquipment.GearSlots)
        {
            UGearData gearCopy = new UGearData();
            gearCopy.Init(gearSlot.Value.GetSaveData());
            m_EquipmentComponent.GearDataList.Add(gearCopy);
        }
    }

    protected override void UpdatePlayerState()
    {
        PlayerState.Get().GetInventoryComponent().Gold += 3000;
        PlayerState.Get().SaveData();
    }
}
