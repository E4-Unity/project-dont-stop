using UnityEngine;
using UnityEngine.UI;

public enum EItemType
{
    Weapon,
    Gear,
    Heal
}
public class Item : MonoBehaviour
{
    #region Component

    Image m_Icon;
    Text m_TextLevel;
    Text m_TextName;
    Text m_TextDescription;

    #endregion
    
    #region Initialization

    [SerializeField] EItemType m_ItemType;
    [SerializeField] UWeaponData m_WeaponData;
    [SerializeField] UGearData m_GearData;
    [SerializeField] ItemData m_Data;

    #endregion

    #region State

    [SerializeField, ReadOnly] Weapon m_Weapon;
    [SerializeField, ReadOnly] Gear m_Gear;

    #endregion

    void Awake()
    {
        // Player Equipment Weapon 확인
        if (m_ItemType == EItemType.Weapon)
        {
            var weaponData = SurvivalGameState.Instance.GetEquipmentComponent().WeaponData;
            if (weaponData is not null && m_WeaponData.WeaponType == weaponData.WeaponType)
                m_WeaponData = weaponData;
        }
        
        m_Icon = GetComponentsInChildren<Image>()[1];
        
        // TODO 계층구조의 순서를 따라가는데 리팩토링 필요할 듯
        Text[] texts = GetComponentsInChildren<Text>();
        m_TextLevel = texts[0];
        m_TextName = texts[1];
        m_TextDescription = texts[2];
        
        switch (m_ItemType)
        {
            case EItemType.Weapon:
                m_Icon.sprite = m_WeaponData.Icon;
                m_TextName.text = m_WeaponData.DisplayName;
                break;
            case EItemType.Gear:
                m_Icon.sprite = m_GearData.Icon;
                m_TextName.text = m_GearData.DisplayName;
                break;
            default:
                m_Icon.sprite = m_Data.ItemIcon;
                m_TextName.text = m_Data.ItemName;
                break;
        }
    }

    void OnEnable()
    {
        if (m_Weapon is null && m_Gear is null)
        {
            switch (m_ItemType)
            {
                case EItemType.Weapon:
                    m_WeaponData.Level = m_WeaponData.Level;
                    m_TextLevel.text = "Lv." + m_WeaponData.Level;
                    if(m_WeaponData.WeaponType == EWeaponType.Melee)
                        m_TextDescription.text = string.Format(m_WeaponData.Description, m_WeaponData.Attribute.Damage, m_WeaponData.Attribute.ProjectileNum);
                    else
                        m_TextDescription.text = string.Format(m_WeaponData.Description, m_WeaponData.Attribute.Damage, m_WeaponData.Attribute.Penetration);
                    break;
                case EItemType.Gear:
                    m_GearData.Level = m_GearData.Level;
                    m_TextLevel.text = "Lv." + m_GearData.Level;
                    float speed = m_GearData.NextAttribute.AttackSpeed != 0
                        ? m_GearData.Attribute.AttackSpeed
                        : m_GearData.Attribute.MovementSpeed;
                    m_TextDescription.text = string.Format(m_GearData.Description, speed * 100);
                    break;
                default:
                    m_TextDescription.text = m_Data.ItemDescription;
                    break;
            }

            return;
        }
        
        switch(m_ItemType)
        {
            case EItemType.Weapon:
                m_TextLevel.text = "Lv." + (m_WeaponData.Level + 1);
                if(m_WeaponData.WeaponType == EWeaponType.Melee)
                    m_TextDescription.text = string.Format(m_WeaponData.Description, m_WeaponData.NextAttribute.Damage, m_WeaponData.NextAttribute.ProjectileNum);
                else
                {
                    int count = m_WeaponData.NextAttribute.ProjectileNum > 0
                        ? m_WeaponData.NextAttribute.ProjectileNum
                        : m_WeaponData.NextAttribute.Penetration;
                    m_TextDescription.text = string.Format(m_WeaponData.Description, m_WeaponData.NextAttribute.Damage, count);
                }
                break;
            case EItemType.Gear:
                m_TextLevel.text = "Lv." + (m_GearData.Level + 1);
                float speed = m_GearData.NextAttribute.AttackSpeed != 0
                    ? m_GearData.NextAttribute.AttackSpeed
                    : m_GearData.NextAttribute.MovementSpeed;
                m_TextDescription.text = string.Format(m_GearData.Description, speed * 100);
                break;
            default:
                m_TextDescription.text = string.Format(m_Data.ItemDescription);
                break;
        }
    }

    public void OnClick()
    {
        switch (m_ItemType)
        {
            case EItemType.Weapon:
                if (m_Weapon is null)
                {
                    GameObject weaponObject = new GameObject(m_WeaponData.DisplayName);
                    m_Weapon = weaponObject.AddComponent<Weapon>();
                    m_Weapon.Init(m_WeaponData);
                    SurvivalGameState.Instance.GetSurvivalEquipmentComponent().AddWeapon(m_Weapon);
                }
                else
                    SurvivalGameState.Instance.GetSurvivalEquipmentComponent().LevelUpWeapon(m_Weapon);
                break;
            case EItemType.Gear:
                if (m_Gear is null)
                {
                    m_GearData.Level = m_GearData.Level; // Refresh
                    GameObject gearObject = new GameObject(m_GearData.DisplayName);
                    m_Gear = gearObject.AddComponent<Gear>();
                    m_Gear.Init(m_GearData);
                    SurvivalGameState.Instance.GetSurvivalEquipmentComponent().AddGear(m_Gear);
                }
                else
                {
                    SurvivalGameState.Instance.GetSurvivalEquipmentComponent().LevelUpGear(m_Gear);
                }
                break;
            case EItemType.Heal:
                SurvivalGameManager.Get().Health = SurvivalGameManager.Get().MaxHealth;
                break;
        }
    }
}
