using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour, ICheckInit
{
    [SerializeField] UWeaponData m_WeaponData;
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

    public void AddWeapon(int _id)
    {
        m_WeaponData = DataManager.Get().GetWeaponData(_id);
        OnWeaponUpdate?.Invoke(m_WeaponData);
    }

    public void AddGear(int _id)
    {
        // 새로운 장비 정보 확인
        var gearData = DataManager.Get().GetGearData(_id);
        var gearType = gearData.Definition.GearType;
            
        // 기존 장비 확인
        if(m_GearSlots.TryGetValue(gearType, out var currentGear))
            m_GearDataList.Remove(currentGear);
            
        // 새로운 장비 추가
        m_GearSlots[gearType] = gearData;
        m_GearDataList.Add(gearData);
        
        OnGearUpdate?.Invoke(gearType, gearData);
    }
    
    // TODO RemoveGear

    public List<int> GetGearIDList()
    {
        var gearIDList = new List<int>();
        foreach (var gearSlot in m_GearSlots)
        {
            gearIDList.Add(gearSlot.Value.DefinitionID);
        }

        return gearIDList;
    }

    #endregion

    #region ICheckInit

    public bool IsInitialized { get; set; }

    List<Action> m_OnInitActions = new List<Action>();
    public List<Action> OnInitActions => m_OnInitActions;

    #endregion
}
