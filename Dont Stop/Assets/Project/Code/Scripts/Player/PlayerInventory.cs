using System;
using System.Collections.Generic;
using Framework;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class JsonInventoryData
{
    public List<FSavedAttributeData> WeaponDataList = new List<FSavedAttributeData>();
    public List<FSavedAttributeData> GearDataList = new List<FSavedAttributeData>();
    
    public int Gold;
    public int Crystal;
}

[Serializable]
public class PlayerInventory : MonoBehaviour, IDataModel, IDataManager
{
    #region Reference

    [SerializeField, ReadOnly] PlayerEquipment m_EquipmentComponent;

    protected PlayerEquipment GetEquipmentComponent() => m_EquipmentComponent;

    #endregion
    
    [SerializeField] int m_Gold;
    [SerializeField] int m_Crystal;
    
    [SerializeReference] List<UAttributeDataBase> m_InventoryList;
    [SerializeField] int m_MaxStack = 10;

    public event Action<int> OnGoldUpdate;
    public event Action<int> OnCrystalUpdate;

    public event Action OnInventoryUpdate;

    #region Property

    public List<UAttributeDataBase> InventoryList => m_InventoryList;

    public bool IsFull => m_MaxStack <= m_InventoryList.Count;
    
    public int Gold
    {
        get => m_Gold;
        set
        {
            m_Gold = value;
            OnGoldUpdate?.Invoke(value);
        }
    }

    public int Crystal
    {
        get => m_Crystal;
        set
        {
            m_Crystal = value;
            OnCrystalUpdate?.Invoke(value);
        }
    }
    
    #endregion

    public void ManualBroadcast()
    {
        OnGoldUpdate?.Invoke(m_Gold);
        OnCrystalUpdate?.Invoke(m_Crystal);
    }

    #region API

    public void Init(PlayerEquipment _playerEquipment)
    {
        m_EquipmentComponent = _playerEquipment;
    }

    public void Equip(UAttributeDataBase _equipment)
    {
        var oldEquipment = GetEquipmentComponent().AddEquipment(_equipment);
        RemoveItem(_equipment);
        
        if (oldEquipment is not null && oldEquipment.DefinitionBase)
            AddItem(oldEquipment);
        
        OnInventoryUpdate?.Invoke();
    }
    
    public bool AddItem(UAttributeDataBase _data)
    {
        if (IsFull) return false;

        m_InventoryList.Add(_data);
        
        OnInventoryUpdate?.Invoke();

        return true;
    }
    
    public bool AddItem(AttributeDefinitionBase _definition)
    {
        return AddItem(_definition.GetAttributeDataBase()); // TODO 리팩토링?
    }
    
    public void RemoveItem(UAttributeDataBase _data)
    {
        m_InventoryList.Remove(_data);
        OnInventoryUpdate?.Invoke();
    }

    public bool CheckItem(AttributeDefinitionBase _definition)
    {
        foreach (var item in m_InventoryList)
        {
            if (item.DefinitionBase == _definition)
                return true;
        }

        return false;
    }

    #endregion

    #region Monobehaviour

    void Awake()
    {
        m_InventoryList = new List<UAttributeDataBase>(m_MaxStack);

        OnGoldUpdate += _i => SaveData();
        OnCrystalUpdate += _i => SaveData();
        OnInventoryUpdate += SaveData;
    }

    #endregion

    #region IDataManager

    public void LoadData()
    {
        var saveData = DataManager.Get().LoadJsonData<JsonInventoryData>("InventoryData", "Config/InventoryData");

        // weapon 추가
        foreach (var weaponData in saveData.WeaponDataList)
        {
            var newWeaponData = new UWeaponData();
            newWeaponData.Init(weaponData);
            m_InventoryList.Add(newWeaponData);
        }
        
        // gear 추가
        foreach (var gearData in saveData.GearDataList)
        {
            var newGearData = new UGearData();
            newGearData.Init(gearData);
            m_InventoryList.Add(newGearData);
        }
        
        // 재화 추가
        Gold = saveData.Gold;
        Crystal = saveData.Crystal;
    }

    public void SaveData()
    {
        // 세이브 데이터 생성
        JsonInventoryData saveData = new JsonInventoryData();
        
        // 인벤토리 아이템 저장
        foreach (var itemData in m_InventoryList)
        {
            switch (itemData)
            {
                // TODO 인터페이스로 대체
                case UWeaponData weaponData:
                    saveData.WeaponDataList.Add(weaponData.GetSaveData());
                    break;
                case UGearData gearData:
                    saveData.GearDataList.Add(gearData.GetSaveData());
                    break;
            }
        }
        
        // 재화 저장
        saveData.Gold = Gold;
        saveData.Crystal = Crystal;

        // 세이브 데이터 저장
        DataManager.Get().Save("InventoryData", JsonUtility.ToJson(saveData));
    }

    #endregion
}
