using System;
using UnityEngine;

namespace Framework
{
    [Serializable]
    public abstract class UAttributeData<TDefinition, TAttribute> : IDataModel where TAttribute : UAttribute<TAttribute>, new() where TDefinition : EntityDefinition<TDefinition, TAttribute>, new()
    {
        #region Model Properties

        #region Level

        [SerializeField] int m_Level;
        public event Action<int> OnLevelUpdate;
        public int Level
        {
            get => m_Level;
            set
            {
                m_Level = value;
                OnLevelUpdate?.Invoke(value);
                NextExp = m_Definition.GetNextExp(m_Level);
                NextGold = m_Definition.GetNextGold(m_Level);
            }
        }

        #endregion

        #region Exp

        [SerializeField] int m_Exp;
        public event Action<int> OnExpUpdate;
        public int Exp
        {
            get => m_Exp;
            set
            {
                m_Exp = value;
                OnExpUpdate?.Invoke(value);
                if (m_Exp >= m_NextExp)
                {
                    Exp -= m_NextExp;
                    Level++;
                }
            }
        }

        #endregion

        #region Next Exp

        [SerializeField] int m_NextExp;
        public event Action<int> OnNextExpUpdate;
        public int NextExp
        {
            get => m_NextExp;
            set
            {
                m_NextExp = value;
                OnNextExpUpdate?.Invoke(value);
            }
        }

        #endregion

        #region NextGold

        [SerializeField] int m_NextGold;
        public event Action<int> OnNextGoldUpdate;
        
        public int NextGold
        {
            get => m_NextGold;
            set
            {
                m_NextGold = value;
                OnNextGoldUpdate?.Invoke(value);
            }
        }

        #endregion

        #region DefinitionID

        [SerializeField] public int m_DefinitionID;
        public event Action<int> OnDefinitionIDUpdate;

        public int DefinitionID
        {
            get => m_DefinitionID;
            set
            {
                m_DefinitionID = value;
                OnDefinitionIDUpdate?.Invoke(value);
                Definition = Array.Find(EntityDefinition<TDefinition, TAttribute>.Definitions,
                    _definition => _definition.ID == DefinitionID);
            }
        }

        #endregion

        #endregion

        #region Properties

        #region Definition

        [SerializeField] TDefinition m_Definition;
        public TDefinition Definition
        {
            get => m_Definition;
            set => m_Definition = value;
        }

        #endregion

        #endregion

        #region API

        public void Init(FSavedAttributeData _savedAttributeData)
        {
            DefinitionID = _savedAttributeData.DefinitionID;
            Level = _savedAttributeData.Level;
            Exp = _savedAttributeData.Exp;
        }
        public void GetSaveData(out FSavedAttributeData _savedAttributeData)
        {
            _savedAttributeData.DefinitionID = DefinitionID;
            _savedAttributeData.Level = Level;
            _savedAttributeData.Exp = Exp;
        }
        public TAttribute GetAttribute() => m_Definition.GetAttribute(m_Level);

        #endregion

        #region IDataModel

        public virtual void ManualBroadcast()
        {
            OnLevelUpdate?.Invoke(m_Level);
            OnExpUpdate?.Invoke(m_Exp);
            OnNextExpUpdate?.Invoke(m_NextExp);
        }

        #endregion
    }
}

[Serializable]
public struct FSavedAttributeData
{
    public int Level;
    public int Exp;
    public int DefinitionID;
}