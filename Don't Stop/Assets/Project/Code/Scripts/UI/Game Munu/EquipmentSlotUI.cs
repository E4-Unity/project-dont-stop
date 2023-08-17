using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlotUI : MonoBehaviour
{
    [SerializeField] Image m_Weapon;
    [SerializeField] Image m_Necklace;
    [SerializeField] Image m_Glove;
    [SerializeField] Image m_Armor;
    [SerializeField] Image m_Belt;
    [SerializeField] Image m_Shoe;

    PlayerEquipment m_EquipmentComponent;

    void OnEnable()
    {
        // 초기화
        m_EquipmentComponent = PlayerState.Get().GetEquipmentComponent();
        if (m_EquipmentComponent.WeaponData?.Definition is not null)
        {
            m_Weapon.sprite = m_EquipmentComponent.WeaponData.Definition.Icon;
            foreach (var gearSlot in m_EquipmentComponent.GearSlots)
            {
                switch (gearSlot.Key)
                {
                    case EGearType.Armor:
                        m_Armor.sprite = gearSlot.Value.Definition.Icon;
                        break;
                    case EGearType.Belt:
                        m_Belt.sprite = gearSlot.Value.Definition.Icon;
                        break;
                    case EGearType.Necklace:
                        m_Necklace.sprite = gearSlot.Value.Definition.Icon;
                        break;
                    case EGearType.Glove:
                        m_Glove.sprite = gearSlot.Value.Definition.Icon;
                        break;
                    case EGearType.Shoe:
                        m_Shoe.sprite = gearSlot.Value.Definition.Icon;
                        break;
                }
            }
        }

        // 이벤트 바인딩
        m_EquipmentComponent.OnWeaponUpdate += OnWeaponUpdate_Event;
        m_EquipmentComponent.OnGearUpdate += OnWeaponUpdate_Event;
    }

    void OnDisable()
    {
        // 이벤트 언바인딩
        m_EquipmentComponent.OnWeaponUpdate -= OnWeaponUpdate_Event;
        m_EquipmentComponent.OnGearUpdate -= OnWeaponUpdate_Event;
    }

    void OnWeaponUpdate_Event(UWeaponData _weaponData)
    {
        m_Weapon.sprite = _weaponData.Definition.Icon;
    }
    
    void OnWeaponUpdate_Event(EGearType _gearType, UGearData _gearData)
    {
        switch (_gearType)
        {
            case EGearType.Armor:
                m_Armor.sprite = _gearData.Definition.Icon;
                break;
            case EGearType.Belt:
                m_Belt.sprite = _gearData.Definition.Icon;
                break;
            case EGearType.Necklace:
                m_Necklace.sprite = _gearData.Definition.Icon;
                break;
            case EGearType.Glove:
                m_Glove.sprite = _gearData.Definition.Icon;
                break;
            case EGearType.Shoe:
                m_Shoe.sprite = _gearData.Definition.Icon;
                break;
        }
    }
}
