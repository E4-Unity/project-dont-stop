using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInventory), typeof(PlayerEquipment), typeof(PlayerStats))]
public class PlayerState : GenericMonoSingleton<PlayerState>, ICheckInit, IManager
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

    public void SelectCharacter(int _id, bool _save = true)
    {
        var characterData = DataManager.Get().GetCharacterData(_id);
        if (characterData is null)
        {
            print("Selected Character is not exist in Data Manager : " + _id);
            return;
        }
        
        m_CharacterData = DataManager.Get().GetCharacterData(_id);
        if(_save)
            DataManager.Get().SaveJsonData();
    }

    #endregion

    #region Monobehaviour

    protected override void Awake()
    {
        base.Awake();
        m_InventoryComponent = GetComponent<PlayerInventory>();
        m_EquipmentComponent = GetComponent<PlayerEquipment>();
        m_StatsComponent = GetComponent<PlayerStats>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DataManager.Get().SaveJsonData();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }

    #endregion

    // TODO 제거 예정
    #region ICheckInit
    
    public bool IsInitialized { get; set; }

    List<Action> m_OnInitActions = new List<Action>();
    public List<Action> OnInitActions => m_OnInitActions;

    #endregion

    public void InitManager()
    {
        
    }
}