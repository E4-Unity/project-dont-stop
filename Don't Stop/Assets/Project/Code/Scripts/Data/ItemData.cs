using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/ItemData")]
public class ItemData : ScriptableObject
{
    public enum ItemType
    {
        Melee,
        Range,
        Glove,
        Shoe,
        Heal
    }
    
    [Header("# Main Info")]
    [SerializeField] ItemType m_ItemType;
    [SerializeField] int m_ItemID;
    [SerializeField] string m_ItemName;
    [SerializeField] [TextArea] string m_ItemDescription;
    [SerializeField] Sprite m_ItemIcon;

    public ItemType Type => m_ItemType;
    public int ItemID => m_ItemID;
    public string ItemName => m_ItemName;
    public string ItemDescription => m_ItemDescription;
    public Sprite ItemIcon => m_ItemIcon;

    [Header("# Level Data")]
    [SerializeField] int m_BaseDamage;
    [SerializeField] int m_BaseCount;
    [SerializeField] float[] m_Damages;
    [SerializeField] int[] m_Counts;
    
    public int BaseDamage => m_BaseDamage;
    public int BaseCount => m_BaseCount;
    public float[] Damages => m_Damages;
    public int[] Counts => m_Counts;

    [Header("# Weapon")]
    [SerializeField] GameObject m_Projectile;
    [SerializeField] Sprite m_WeaponSprite;

    public GameObject Projectile => m_Projectile;
    public Sprite WeaponSprite => m_WeaponSprite;
}
