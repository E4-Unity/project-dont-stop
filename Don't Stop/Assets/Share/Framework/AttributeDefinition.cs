using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Framework
{
    public class AttributeDefinition<TDefinition, TAttribute> : ScriptableObject where TAttribute : UAttribute<TAttribute>, new() where TDefinition : AttributeDefinition<TDefinition, TAttribute>
    {
        public static TDefinition[] Definitions;

        [SerializeField] TAttribute m_BaseAttribute;
        [SerializeField] TAttribute[] m_NextAttributes;
        [SerializeField] int[] m_NextExp;
        [SerializeField] int[] m_NextGold;
    
        /* API */
        public int GetNextExp(int _level) => _level > m_NextExp.Length - 1 ? m_NextExp.Last() : m_NextExp[_level];
        public int GetNextGold(int _level) => _level > m_NextGold.Length - 1 ? m_NextGold.Last() : m_NextGold[_level];
    
        public TAttribute GetNextAttribute(int _level)
        {
            // 레벨이 0이거나 레벨업에 따른 추가 능력치가 없는 경우
            if (_level == 0 || m_NextAttributes.Length == 0) return new TAttribute();
        
            return _level > m_NextAttributes.Length - 1 ? m_NextAttributes.Last() : m_NextAttributes[_level];
        }

        // TODO 최적화 필요
        public TAttribute GetAttribute(int _level)
        {
            TAttribute result = m_BaseAttribute;
            for (int i = 1; i <= _level; i++)
            {
                result += GetNextAttribute(i);
            }

            return result;
        }
    }
}