using System;
using Framework;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Gear Definition", menuName = "Scriptable Object/Attribute Definition/Gear")]
public class GearDefinition : EntityDefinition<GearDefinition, UBasicAttribute>
{
    [SerializeField] EGearType m_GearType;
    public EGearType GearType => m_GearType;
    public override UAttributeDataBase GetAttributeDataBase()
    {
        return new UGearData()
        {
            Definition = this,
            Level = 0
        };
    }
}

public enum EGearType
{
    Armor,
    Glove,
    Shoe,
    Necklace,
    Belt
}