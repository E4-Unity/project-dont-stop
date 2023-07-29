using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    [SerializeField] ItemData.ItemType m_Type;
    [SerializeField] float m_Rate;

    public void Init(ItemData _data)
    {
        // Basic Set
        name = "Gear " + _data.ItemID;
        transform.parent = GameManager.Get().GetPlayer().transform;
        transform.localPosition = Vector3.zero;

        // Property Set
        m_Type = _data.Type;
        m_Rate = _data.Damages[0];
        ApplyGear();
    }

    public void LevelUp(float _rate)
    {
        m_Rate = _rate;
        ApplyGear();
    }

    void ApplyGear()
    {
        switch (m_Type)
        {
            case ItemData.ItemType.Glove:
                RateUp();
                break;
            case ItemData.ItemType.Shoe:
                SpeedUp();
                break;
        }
    }

    void RateUp()
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        foreach (Weapon weapon in weapons)
        {
            switch (weapon.WeaponID)
            {
                // 근접 무기
                case 0:
                    float speed = 150 * Character.WeaponSpeed;
                    weapon.Speed = speed + (speed * m_Rate);
                    break;
                // 원거리 무기
                default:
                    float rate = 0.3f * Character.WeaponRate;
                    weapon.Speed = rate * (1f - m_Rate);
                    break;
            }
        }
    }

    void SpeedUp()
    {
        float speed = 3f * Character.Speed;
        GameManager.Get().GetPlayer().Speed = speed + speed * m_Rate;
    }
}
