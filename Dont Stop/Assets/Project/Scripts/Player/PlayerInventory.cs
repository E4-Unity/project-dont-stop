using System;
using System.Collections.Generic;
using E4.Utility;
using Framework;
using UnityEngine;

[Serializable]
public class InventorySaveData
{
    public List<FSavedAttributeData> WeaponDataList = new List<FSavedAttributeData>();
    public List<FSavedAttributeData> GearDataList = new List<FSavedAttributeData>();
    
    public int Gold;
    public int Crystal;
}

[Serializable]
public class PlayerInventory : MonoBehaviour, IDataModel, ISavable<InventorySaveData>
{
    /* 컴포넌트 */
    PlayerEquipment m_EquipmentComponent;

    protected PlayerEquipment GetEquipmentComponent() => m_EquipmentComponent;
    
    /* 필드 */
    [SerializeField] int m_Gold;
    [SerializeField] int m_Crystal;
    
    [SerializeReference] List<UAttributeDataBase> m_InventoryList;
    [SerializeField] int m_MaxStack = 10;
    
    // ISavable
    InventorySaveData saveData;

    /* 이벤트 */
    public event Action<int> OnGoldUpdate;
    public event Action<int> OnCrystalUpdate;

    public event Action OnInventoryUpdate;
    
    /* 프로퍼티 */
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
    
    /* MonoBehaviour */
    void Awake()
    {
        m_InventoryList = new List<UAttributeDataBase>(m_MaxStack);

        OnGoldUpdate += amount =>
        {
            if (saveData.Gold != amount)
            {
                SaveData();
            }
        };
        OnCrystalUpdate += amount =>
        {
            if (saveData.Crystal != amount)
            {
                SaveData();
            }
        };
        OnInventoryUpdate += SaveData;
    }

    /* API */
    public void ManualBroadcast()
    {
        OnGoldUpdate?.Invoke(m_Gold);
        OnCrystalUpdate?.Invoke(m_Crystal);
    }

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
    
    /* IDataManager 인터페이스 */
    public void LoadData()
    {
        saveData = DataManager.LoadData(this) ?? new InventorySaveData();

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
        // 데이터 저장 요청
        if (!DataManager.RequestSaveData(this)) return;
        
        // 세이브 데이터 생성
        InventorySaveData newSaveData = new InventorySaveData();
        
        // 인벤토리 아이템 저장
        foreach (var itemData in m_InventoryList)
        {
            switch (itemData)
            {
                // TODO 인터페이스로 대체
                case UWeaponData weaponData:
                    newSaveData.WeaponDataList.Add(weaponData.GetSaveData());
                    break;
                case UGearData gearData:
                    newSaveData.GearDataList.Add(gearData.GetSaveData());
                    break;
            }
        }
        
        // 재화 저장
        newSaveData.Gold = Gold;
        newSaveData.Crystal = Crystal;

        // 세이브 데이터 저장
        saveData = newSaveData;
    }

    /* ISavable 인터페이스 */
    public InventorySaveData Data => saveData;

    public bool IsDirty { get; set; }
}
