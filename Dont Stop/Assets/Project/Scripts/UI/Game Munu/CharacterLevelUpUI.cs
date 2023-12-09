using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterLevelUpUI : PollingUI
{
    /* 컴포넌트 */
    PlayerState m_PlayerState;
    
    // Attribute
    [SerializeField] Text m_CharacterNextAttack;
    [SerializeField] Text m_CharacterNextDefense;
    [SerializeField] Text m_CharacterNextMaxHealth;
    [SerializeField] Text m_CharacterNextAttackSpeed;
    [SerializeField] Text m_CharacterNextMoveSpeed;
    
    // CharacterData
    [SerializeField] Text m_CharacterExp;
    [SerializeField] Text m_CharacterCost;
    
    [SerializeField] CharacterDefinition m_CharacterDefinition;

    protected override void Awake_Event()
    {
        base.Awake_Event();

        m_PlayerState = GlobalGameManager.Instance.GetPlayerState();
    }

    protected override void Refresh()
    {
        if (m_PlayerState is null) return;
        int nextLevel = m_PlayerState.CharacterData.Level + 1;
        m_CharacterDefinition = m_PlayerState.CharacterData.Definition;

        // Attribute
        UBasicAttribute basicAttribute = m_CharacterDefinition.GetNextAttribute(nextLevel);

        if(m_CharacterNextAttack)
            m_CharacterNextAttack.text = basicAttribute.Attack > 0
                ? $"(+{basicAttribute.Attack:D0})"
                : "";
        
        if(m_CharacterNextDefense)
            m_CharacterNextDefense.text = basicAttribute.Defense > 0
                ? $"(+{basicAttribute.Defense:D0})"
                : "";
        
        if(m_CharacterNextMaxHealth)
            m_CharacterNextMaxHealth.text = basicAttribute.MaxHealth > 0
                ? $"(+{basicAttribute.MaxHealth:D0})"
                : "";
        if(m_CharacterNextAttackSpeed)
            m_CharacterNextAttackSpeed.text = basicAttribute.AttackSpeed > 0
                ? $"(+{basicAttribute.AttackSpeed:F2})"
                : "";
        
        if(m_CharacterNextMoveSpeed)
            m_CharacterNextMoveSpeed.text = basicAttribute.MovementSpeed > 0
                ? $"(+{basicAttribute.MovementSpeed:F2})"
                : "";
        if (m_CharacterExp)
            m_CharacterExp.text = $"Exp {m_PlayerState.CharacterData.Exp}/{m_PlayerState.CharacterData.NextExp}";

        if (m_CharacterCost)
            m_CharacterCost.text = $"{m_PlayerState.CharacterData.NextGold}";
    }
}
