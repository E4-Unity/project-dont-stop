using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentComponentBoss : MonoBehaviour
{
    [SerializeField] List<WeaponBoss> m_Weapons = new List<WeaponBoss>();
    [SerializeField] List<GearBoss> m_Gears = new List<GearBoss>();

    [SerializeField] UBasicAttribute m_TotalAttributes;

    public void Init()
    {
        CalculateTotalAttributes();
    }

    public void AddWeapon(WeaponBoss _weapon)
    {
        m_Weapons.Add(_weapon);
        ApplyTotalAttributes(_weapon);
    }

    public void AddGear(GearBoss _gear)
    {
        m_Gears.Add(_gear);
        CalculateTotalAttributes();
        ApplyTotalAttributesAll();
    }


    void CalculateTotalAttributes()
    {
        m_TotalAttributes = new UBasicAttribute();

        foreach (var gear in m_Gears)
        {
            m_TotalAttributes += gear.GearData.Attribute;
        }

        m_TotalAttributes += GameStateBoss.Get().GetStatsComponent().TotalStats;
    }

    void ApplyTotalAttributes(WeaponBoss _weapon)
    {
        _weapon.Damage = (int)(_weapon.BaseDamage * (1 + m_TotalAttributes.Attack / 100.0f));
        switch (_weapon.WeaponData.WeaponType)
        {
            case EWeaponType.Melee:
                _weapon.Speed = _weapon.BaseSpeed * m_TotalAttributes.AttackSpeed;
                break;
            default:
                _weapon.Speed = _weapon.BaseSpeed / m_TotalAttributes.AttackSpeed;
                break;
        }
    }

    void ApplyTotalAttributesAll()
    {
        foreach (var weapon in m_Weapons)
        {
            ApplyTotalAttributes(weapon);
        }
    }
}
