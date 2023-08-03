using System;
using System.Collections.Generic;
using Presenter;
using UnityEngine;

namespace Presenter
{
    public interface IInventory
    {
        public int Gold { set; }
        public int Crystal { set; }
    }

    public interface IPlayerData
    {
        public string PlayerName { set; }
        public int PlayerLevel { set; }
        public int PlayerExp { set; }
        public int PlayerNextExp { set; }
    }

    public interface IPlayerStats
    {
        public int MaxHealth { set; }
        public int Attack { set; }
    }
}

public interface IDataModel
{
    public void ManualBroadcast();
}

public class PlayerPresenter : MonoBehaviour
{
    #region Field

    #region Data Models

    [SerializeField] PlayerInventory m_InventoryModel;
    [SerializeField] UPlayerData m_PlayerData;
    [SerializeField] UBasicAttribute m_PlayerStats;

    #endregion
    
    #region Views

    [SerializeField] GameObject[] m_Views;
    List<IInventory> m_InventoryViews = new List<IInventory>();
    List<IPlayerData> m_PlayerDataViews = new List<IPlayerData>();
    List<IPlayerStats> m_PlayerStatsViews = new List<IPlayerStats>();

    #endregion

    #endregion

    #region Event Functions

    #region Inventory

    void OnGoldUpdate_Event(int _value)
    {
        foreach (var inventoryView in m_InventoryViews)
        {
            inventoryView.Gold = _value;
        }
    }
    
    void OnCrystalUpdate_Event(int _value)
    {
        foreach (var inventoryView in m_InventoryViews)
        {
            inventoryView.Crystal = _value;
        }
    }

    #endregion

    #region PlayerData

    void OnPlayerNameUpdate_Event(string _value)
    {
        foreach (var playerDataView in m_PlayerDataViews)
        {
            playerDataView.PlayerName = _value;
        }
    }

    void OnPlayerLevelUpdate_Event(int _value)
    {
        foreach (var playerDataView in m_PlayerDataViews)
        {
            playerDataView.PlayerLevel = _value;
        }
    }
    
    void OnPlayerExpUpdate_Event(int _value)
    {
        foreach (var playerDataView in m_PlayerDataViews)
        {
            playerDataView.PlayerExp = _value;
        }
    }
    
    void OnPlayerNextExpUpdate_Event(int _value)
    {
        foreach (var playerDataView in m_PlayerDataViews)
        {
            playerDataView.PlayerNextExp = _value;
        }
    }

    #endregion

    #region Player Stats

    void OnMaxHealthUpdate_Event(int _value)
    {
        foreach (var playerStatsView in m_PlayerStatsViews)
        {
            playerStatsView.MaxHealth = _value;
        }
    }

    void OnAttackUpdate_Event(int _value)
    {
        foreach (var playerStatsView in m_PlayerStatsViews)
        {
            playerStatsView.Attack = _value;
        }
    }

    #endregion

    #endregion

    #region Method

    void BindInventoryView()
    {
        m_InventoryModel = PlayerState.Get().GetInventoryComponent();
        m_InventoryModel.OnGoldUpdate += OnGoldUpdate_Event;
        m_InventoryModel.OnCrystalUpdate += OnCrystalUpdate_Event;
        m_InventoryModel.ManualBroadcast();
    }

    void UnbindInventoryView()
    {
        m_InventoryModel.OnGoldUpdate -= OnGoldUpdate_Event;
        m_InventoryModel.OnCrystalUpdate -= OnCrystalUpdate_Event;
    }
    
    void BindPlayerStatsView()
    {
        m_PlayerStats = PlayerState.Get().GetStatsComponent().TotalStats;
        m_PlayerStats.OnMaxHealthUpdate += OnMaxHealthUpdate_Event;
        m_PlayerStats.OnAttackUpdate += OnAttackUpdate_Event;
        m_PlayerStats.ManualBroadcast();
    }
    
    void UnbindPlayerStatsView()
    {
        m_PlayerStats.OnMaxHealthUpdate -= OnMaxHealthUpdate_Event;
        m_PlayerStats.OnAttackUpdate -= OnAttackUpdate_Event;
    }

    void BindPlayerDataView()
    {
        m_PlayerData = PlayerState.Get().PlayerData;
        m_PlayerData.OnPlayerNameUpdate += OnPlayerNameUpdate_Event;
        m_PlayerData.OnLevelUpdate += OnPlayerLevelUpdate_Event;
        m_PlayerData.OnExpUpdate += OnPlayerExpUpdate_Event;
        m_PlayerData.OnNextExpUpdate += OnPlayerNextExpUpdate_Event;
        m_PlayerData.ManualBroadcast();
    }
    
    void UnbindPlayerDataView()
    {
        m_PlayerData.OnPlayerNameUpdate -= OnPlayerNameUpdate_Event;
        m_PlayerData.OnLevelUpdate -= OnPlayerLevelUpdate_Event;
        m_PlayerData.OnExpUpdate -= OnPlayerExpUpdate_Event;
        m_PlayerData.OnNextExpUpdate -= OnPlayerNextExpUpdate_Event;
    }

    #endregion

    #region MonoBehaviour

    void Awake()
    {
        // 프로퍼티 바인딩을 위한 목록 검색
        foreach (var view in m_Views)
        {
            // Inventory Views
            var inventoryView = view.GetComponent<IInventory>();
            if (inventoryView is not null) m_InventoryViews.Add(inventoryView);
            
            // Player Data Views
            var playerDataView = view.GetComponent<IPlayerData>();
            if (playerDataView is not null) m_PlayerDataViews.Add(playerDataView);
            
            // Player Stats Views
            var playerStatsView = view.GetComponent<IPlayerStats>();
            if (playerStatsView is not null) m_PlayerStatsViews.Add(playerStatsView);
        }
    }

    void OnEnable()
    {
        BindInventoryView();
        BindPlayerDataView();
        BindPlayerStatsView();
    }

    void OnDisable()
    {
        UnbindInventoryView();
        UnbindPlayerDataView();
        UnbindPlayerStatsView();
    }

    #endregion
}
