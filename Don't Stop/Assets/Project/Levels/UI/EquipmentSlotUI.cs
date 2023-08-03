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

    void OnEnable()
    {
        // 초기화
        var equipmentComponent = PlayerState.Get().GetEquipmentComponent();
        if (equipmentComponent.IsInitialized)
        {
            m_Weapon.sprite = equipmentComponent.WeaponData.Definition.Icon;
            foreach (var gearSlot in equipmentComponent.GearSlots)
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
        equipmentComponent.OnWeaponUpdate += (_weaponData) => { m_Weapon.sprite = _weaponData.Definition.Icon; };
        equipmentComponent.OnGearUpdate += ((_type, _data) =>
        {
            switch (_type)
            {
                case EGearType.Armor:
                    m_Armor.sprite = _data.Definition.Icon;
                    break;
                case EGearType.Belt:
                    m_Belt.sprite = _data.Definition.Icon;
                    break;
                case EGearType.Necklace:
                    m_Necklace.sprite = _data.Definition.Icon;
                    break;
                case EGearType.Glove:
                    m_Glove.sprite = _data.Definition.Icon;
                    break;
                case EGearType.Shoe:
                    m_Shoe.sprite = _data.Definition.Icon;
                    break;
            }
        });
    }
}
