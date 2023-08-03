using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;

public abstract class EntityDefinition<TDefinition, TAttribute> : AttributeDefinition<TDefinition, TAttribute> where TAttribute : UAttribute<TAttribute>, new() where TDefinition : AttributeDefinition<TDefinition, TAttribute>
{
    [SerializeField] int m_ID;
    [SerializeField] string m_DisplayName;
    [SerializeField] string m_Description;
    [SerializeField] Sprite m_Icon;
    
    public int ID => m_ID;
    public string DisplayName => m_DisplayName;

    public string Description => m_Description;
    public Sprite Icon => m_Icon;
}
