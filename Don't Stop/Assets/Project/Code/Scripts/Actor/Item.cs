using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    /* 레퍼런스 */
    Image m_Icon;
    Text m_TextLevel;
    Text m_TextName;
    Text m_TextDescription;
    
    /* 필드 */
    [SerializeField] ItemData m_Data;
    [SerializeField] int m_Level;
    [SerializeField, ReadOnly] Weapon m_Weapon;
    [SerializeField, ReadOnly] Gear m_Gear;

    /* 프로퍼티 */
    public int Level => m_Level;
    public ItemData Data => m_Data;

    void Awake()
    {
        m_Icon = GetComponentsInChildren<Image>()[1];
        m_Icon.sprite = m_Data.ItemIcon;

        // TODO 계층구조의 순서를 따라가는데 리팩토링 필요할 듯
        Text[] texts = GetComponentsInChildren<Text>();
        m_TextLevel = texts[0];
        m_TextName = texts[1];
        m_TextDescription = texts[2];

        // 초기화
        m_TextName.text = m_Data.ItemName;
    }

    void OnEnable()
    {
        m_TextLevel.text = "Lv." + m_Level;

        switch(m_Data.Type)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                m_TextDescription.text = string.Format(m_Data.ItemDescription, m_Data.Damages[m_Level] * 100, m_Data.Counts[m_Level]);
                break;
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                m_TextDescription.text = string.Format(m_Data.ItemDescription, m_Data.Damages[m_Level] * 100);
                break;
            default:
                m_TextDescription.text = string.Format(m_Data.ItemDescription);
                break;
        }
    }

    public void OnClick()
    {
        switch (m_Data.Type)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                if (m_Level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    m_Weapon = newWeapon.AddComponent<Weapon>();
                    m_Weapon.Init(m_Data);
                }
                else
                {
                    float nextDamage = m_Data.BaseDamage * (1 + m_Data.Damages[m_Level]);
                    int nextCount = m_Data.Counts[m_Level];
                    
                    m_Weapon.LevelUp(Mathf.FloorToInt(nextDamage), nextCount);
                }
                m_Level++;
                break;
            case ItemData.ItemType.Glove:
            case ItemData.ItemType.Shoe:
                if (m_Level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    m_Gear = newWeapon.AddComponent<Gear>();
                    m_Gear.Init(m_Data);
                }
                else
                {
                    float nextRate = m_Data.Damages[m_Level];
                    m_Gear.LevelUp(nextRate);
                }
                m_Level++;
                break;
            case ItemData.ItemType.Heal:
                GameManager.Get().Health = GameManager.Get().MaxHealth;
                break;
        }

        if (m_Level == m_Data.Damages.Length)
        {
            GetComponent<Button>().interactable = false;
        }
    }
}
