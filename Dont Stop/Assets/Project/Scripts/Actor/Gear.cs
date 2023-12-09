using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    [SerializeField] ItemData.ItemType m_Type;
    //[SerializeField] float m_Rate;
    [SerializeField] UGearData m_GearData;
    public UGearData GearData => m_GearData;

    public void Init(ItemData _data)
    {
        // Basic Set
        name = "Gear " + _data.ItemID;
        transform.parent = SurvivalGameManager.Get().GetPlayer().transform;
        transform.localPosition = Vector3.zero;

        // Property Set
        m_Type = _data.Type;
        //m_Rate = _data.Damages[0];
        ApplyGear();
    }

    public void Init(UGearData _gearData)
    {
        m_GearData = _gearData;
        
        // Basic Set
        name = "Gear " + m_GearData.DisplayName;
        transform.parent = SurvivalGameManager.Get().GetPlayer().transform;
        transform.localPosition = Vector3.zero;
        
        ApplyGear();
    }

    public void LevelUp()
    {
        m_GearData.Level++;
        ApplyGear();
    }

    public void ApplyGear()
    {
        RateUp();
        SpeedUp();
    }

    void RateUp()
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        foreach (Weapon weapon in weapons)
        {
            weapon.ApplyWeaponAttribute();
            
            switch (weapon.WeaponData.WeaponType)
            {
                // 근접 무기
                case EWeaponType.Melee:
                    weapon.Speed *= (1 + m_GearData.Attribute.AttackSpeed);
                    break;
                // 원거리 무기
                default:
                    weapon.Speed /= (1 + m_GearData.Attribute.AttackSpeed);
                    break;
            }
        }
    }

    void SpeedUp()
    {
        float speed = 3f * (SurvivalGameState.Instance.GetStatsComponent().TotalStats.MovementSpeed + m_GearData.Attribute.MovementSpeed);
        SurvivalGameManager.Get().GetPlayer().Speed = speed;
    }
}
