using System;
using Framework;
using UnityEngine;

public enum EWeaponType
{
    Melee,
    Range
}

[Serializable]
[CreateAssetMenu(fileName = "Weapon Definition", menuName = "Scriptable Object/Attribute Definition/Weapon")]
public class WeaponDefinition : EntityDefinition<WeaponDefinition, UWeaponAttribute>
{
    [SerializeField] GameObject m_Prefab;

    public GameObject Prefab => m_Prefab;

    [SerializeField] Sprite m_DisplaySprite;

    public Sprite DisplaySprite => m_DisplaySprite;
    
    [SerializeField] Sprite m_BulletSprite;

    public Sprite BulletSprite => m_BulletSprite;
    public override UAttributeDataBase GetAttributeDataBase()
    {
        return new UWeaponData()
        {
            Definition = this,
            Level = 0
        };
    }
    
    [SerializeField] EWeaponType m_WeaponType;
    public EWeaponType WeaponType => m_WeaponType;
}