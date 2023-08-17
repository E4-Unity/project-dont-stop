using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Framework;
using UnityEngine;

[Serializable]
public class JsonEquipmentData
{
    public FSavedAttributeData WeaponData = new FSavedAttributeData();
    public List<FSavedAttributeData> GearDataList = new List<FSavedAttributeData>();
}

public class PlayerEquipment : MonoBehaviour, IDataManager
{
    #region Reference

    [SerializeField, ReadOnly] PlayerInventory m_InventoryComponent;

    protected PlayerInventory GetInventoryComponent() => m_InventoryComponent;

    #endregion
    
    [SerializeField] UWeaponData m_WeaponData = new UWeaponData();
    [SerializeField] List<UGearData> m_GearDataList = new List<UGearData>(); // Inspector 디버깅용
    Dictionary<EGearType, UGearData> m_GearSlots = new Dictionary<EGearType, UGearData>();

    public List<UGearData> GearDataList => m_GearDataList;

    public UWeaponData WeaponData
    {
        get => m_WeaponData;
        set => m_WeaponData = value;
    }
    public Dictionary<EGearType, UGearData> GearSlots => m_GearSlots;

    public event Action<UWeaponData> OnWeaponUpdate;
    public event Action<EGearType, UGearData> OnGearUpdate;

    #region API
    
    public void Init(PlayerInventory _playerInventory)
    {
        m_InventoryComponent = _playerInventory;
    }

    public UAttributeDataBase AddEquipment(UAttributeDataBase _equipment)
    {
        UAttributeDataBase oldEquipment = null;

        if (_equipment is UWeaponData weaponData) // 무기인 경우
        {
            oldEquipment = AddWeapon(weaponData);
        }
        else if (_equipment is UGearData gearData) // 기어 장비인 경우
        {
            oldEquipment = AddGear(gearData);
        }

        return oldEquipment;
    }

    public UWeaponData AddWeapon(UWeaponData _weaponData)
    {
        var weaponData = m_WeaponData;
        m_WeaponData = _weaponData;
        
        OnWeaponUpdate?.Invoke(m_WeaponData);
        
        return weaponData;
    }
    
    public UGearData AddGear(UGearData _gearData)
    {
        // 새로운 장비 정보 확인
        var gearType = _gearData.Definition.GearType;

        // 기존 장비 확인
        if(m_GearSlots.TryGetValue(gearType, out var oldGear))
            m_GearDataList.Remove(oldGear);
            
        // 새로운 장비 추가
        m_GearSlots[gearType] = _gearData;
        m_GearDataList.Add(_gearData);
        
        OnGearUpdate?.Invoke(gearType, _gearData);

        return oldGear;
    }

    public bool CheckItem(AttributeDefinitionBase _definition)
    {
        // 무기 확인
        if (_definition == WeaponData.Definition)
            return true;
        
        // 장비 확인
        foreach (var gearData in m_GearDataList)
        {
            if (_definition == gearData.Definition)
                return true;
        }

        return false;
    }

    // TODO RemoveGear

    #endregion

    #region Monobehaviour

    void Awake()
    {
        OnWeaponUpdate += _data => SaveData();
        OnGearUpdate += (_type, _data) => SaveData();
    }

    #endregion

    #region IDataManager

    public void LoadData()
    {
        var saveData = DataManager.Get().LoadJsonData<JsonEquipmentData>("EquipmentData", "Config/EquipmentData");

        m_WeaponData = new UWeaponData();
        m_WeaponData.Init(saveData.WeaponData);

        foreach (var savedGearData in saveData.GearDataList)
        {
            var gearData = new UGearData();
            gearData.Init(savedGearData);
            m_GearSlots[gearData.Definition.GearType] = gearData;
            m_GearDataList.Add(gearData);
        }
    }

    public void SaveData()
    {
        // 세이브 데이터 생성
        JsonEquipmentData saveData = new JsonEquipmentData();

        // 장비 창 정보 저장
        if (WeaponData.Definition is not null)
            saveData.WeaponData = WeaponData.GetSaveData();

        foreach (var gearData in m_GearDataList)
        {
            saveData.GearDataList.Add(gearData.GetSaveData());   
        }

        // 세이브 데이터 저장
        DataManager.Get().Save("EquipmentData", JsonUtility.ToJson(saveData));
    }

    #endregion
}
