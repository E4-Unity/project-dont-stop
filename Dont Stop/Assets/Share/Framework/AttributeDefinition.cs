using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Framework
{
    public abstract class AttributeDefinitionBase : ScriptableObject
    {
        // 레벨 1(_level = 0)일 때 Attribute
        public abstract UAttributeBase GetAttributeBase();
        
        // 레벨업 시 증가하는 Attribute
        public abstract UAttributeBase GetNextAttributeBase(int _level);
        
        // 현재 Attribute 총합
        public abstract UAttributeBase GetTotalAttributeBase(int _level);
        
        // 기본 AttributeData 생성
        public abstract UAttributeDataBase GetAttributeDataBase();
    }
    public abstract class AttributeDefinition<TDefinition, TAttribute> : AttributeDefinitionBase where TAttribute : UAttribute<TAttribute>, new() where TDefinition : AttributeDefinition<TDefinition, TAttribute>
    {
        public static TDefinition[] Definitions;

        [SerializeField] TAttribute m_BaseAttribute;
        [SerializeField] TAttribute[] m_NextAttributes;
        [SerializeField] int[] m_NextExp;
        [SerializeField] int[] m_NextGold;
    
        /* API */
        public int GetNextExp(int _level) => _level > m_NextExp.Length - 1 ? m_NextExp.Last() : m_NextExp[_level];
        public int GetNextGold(int _level) => _level > m_NextGold.Length - 1 ? m_NextGold.Last() : m_NextGold[_level];

        #region API

        public TAttribute GetAttribute(int _level) => GetAttributeBase() as TAttribute;
        public TAttribute GetNextAttribute(int _level) => GetNextAttributeBase(_level) as TAttribute;
        public TAttribute GetTotalAttribute(int _level) => GetTotalAttributeBase(_level) as TAttribute;

        #endregion

        #region AttributeDefinitionBase

        public override UAttributeBase GetAttributeBase() => m_BaseAttribute;
        public override UAttributeBase GetNextAttributeBase(int _level)
        {
            // 레벨업에 따른 추가 능력치가 없는 경우
            if (m_NextAttributes.Length == 0) return new TAttribute();
        
            return _level > m_NextAttributes.Length - 1 ? m_NextAttributes.Last() : m_NextAttributes[_level];
        }

        public override UAttributeBase GetTotalAttributeBase(int _level)
        {
            // 레벨 1인 경우
            if (_level == 0) return m_BaseAttribute;
            
            // 레벨이 1보다 큰 경우
            TAttribute result = m_BaseAttribute;
            
            for (int i = 0; i < _level; i++)
            {
                result += GetNextAttributeBase(i);
            }

            return result;
        }

        #endregion
    }
}