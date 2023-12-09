using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalEquipmentComponent : MonoBehaviour
{
    [SerializeField] List<Weapon> m_Weapons = new List<Weapon>();
    [SerializeField] List<Gear> m_Gears = new List<Gear>();

    [SerializeField] UBasicAttribute m_TotalAttributes;

    public void Init()
    {
        CalculateTotalAttributes();
    }

    public void AddWeapon(Weapon _weapon)
    {
        m_Weapons.Add(_weapon);
        ApplyTotalAttributes(_weapon);
    }

    public void LevelUpWeapon(Weapon _weapon)
    {
        _weapon.LevelUp();
        ApplyTotalAttributes(_weapon);
    }

    public void AddGear(Gear _gear)
    {
        m_Gears.Add(_gear);
        CalculateTotalAttributes();
        ApplyTotalAttributesAll();
    }

    public void LevelUpGear(Gear _gear)
    {
        _gear.LevelUp();
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

        m_TotalAttributes += SurvivalGameState.Instance.GetStatsComponent().TotalStats;
    }
    
    void ApplyTotalAttributes(Weapon _weapon)
    {
        _weapon.Damage = (int)(_weapon.BaseDamage * (1 + m_TotalAttributes.Attack / 100.0f));
        switch (_weapon.WeaponData.WeaponType)
        {
            // 근접 무기
            case EWeaponType.Melee:
                _weapon.Speed = _weapon.BaseSpeed * m_TotalAttributes.AttackSpeed;
                break;
            // 원거리 무기
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
