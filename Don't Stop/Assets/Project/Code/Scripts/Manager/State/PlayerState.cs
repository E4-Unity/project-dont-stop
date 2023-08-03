using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : GenericMonoSingleton<PlayerState>, ICheckInit
{
    #region Components

    PlayerInventory m_InventoryComponent;
    PlayerEquipment m_EquipmentComponent;
    PlayerStats m_StatsComponent;

    public PlayerInventory GetInventoryComponent() => m_InventoryComponent;

    public PlayerEquipment GetEquipmentComponent() => m_EquipmentComponent;

    public PlayerStats GetStatsComponent() => m_StatsComponent;

    #endregion
    
    #region State
    
    [SerializeField] UPlayerData m_PlayerData;
    [SerializeField] UCharacterData m_CharacterData;

    #endregion

    #region Properties

    public UPlayerData PlayerData
    {
        get => m_PlayerData;
        set => m_PlayerData = value;
    }
    
    public UCharacterData CharacterData
    {
        get => m_CharacterData;
        set => m_CharacterData = value;
    }

    #endregion

    #region API

    public void SelectCharacter(int _id)
    {
        m_CharacterData = DataManager.Get().GetCharacterData(_id);
    }

    #endregion

    #region Monobehaviour

    protected override void Awake_Implementation()
    {
        base.Awake_Implementation();
        m_InventoryComponent = GetComponent<PlayerInventory>();
        m_EquipmentComponent = GetComponent<PlayerEquipment>();
        m_StatsComponent = GetComponent<PlayerStats>();
    }

    #endregion

    #region ICheckInit
    
    public bool IsInitialized { get; set; }

    List<Action> m_OnInitActions = new List<Action>();
    public List<Action> OnInitActions => m_OnInitActions;

    #endregion
}