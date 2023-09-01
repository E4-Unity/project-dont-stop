using System;
using Framework;
using UnityEngine;

[Serializable]
public class UBasicAttribute : UAttribute<UBasicAttribute>, IDataModel
{
    #region Properties

    #region Attack
    [SerializeField] public int m_Attack;
    public event Action<int> OnAttackUpdate;
    public int Attack
    {
        get => m_Attack;
        set
        {
            m_Attack = value;
            OnAttackUpdate?.Invoke(value);
        }
    }
    #endregion

    #region Defense

    [SerializeField] public int m_Defense;
    public event Action<int> OnDefenseUpdate;

    public int Defense
    {
        get => m_Defense;
        set
        {
            m_Defense = value;
            OnDefenseUpdate?.Invoke(value);
        }
    }

    #endregion

    #region MaxHealth

    [SerializeField] public int m_MaxHealth;
    public event Action<int> OnMaxHealthUpdate;

    public int MaxHealth
    {
        get => m_MaxHealth;
        set
        {
            m_MaxHealth = value;
            OnMaxHealthUpdate?.Invoke(value);
        }
    }

    #endregion

    #region MovementSpeed

    [SerializeField] public float m_MovementSpeed;
    public event Action<float> OnMovementSpeedUpdate;

    public float MovementSpeed
    {
        get => m_MovementSpeed;
        set
        {
            m_MovementSpeed = value;
            OnMovementSpeedUpdate?.Invoke(value);
        }
    }

    #endregion

    #region AttackSpeed

    [SerializeField] public float m_AttackSpeed;
    public event Action<float> OnAttackSpeedUpdate;

    public float AttackSpeed
    {
        get => m_AttackSpeed;
        set
        {
            m_AttackSpeed = value;
            OnAttackSpeedUpdate?.Invoke(value);
        }
    }

    #endregion
    
    #endregion

    #region IDataModel

    public void ManualBroadcast()
    {
        OnAttackUpdate?.Invoke(m_Attack);
        OnDefenseUpdate?.Invoke(m_Defense);
        OnMaxHealthUpdate?.Invoke(m_MaxHealth);
        OnMovementSpeedUpdate?.Invoke(m_MovementSpeed);
        OnAttackSpeedUpdate?.Invoke(m_AttackSpeed);
    }

    #endregion
    
    public UBasicAttribute() {}

    public UBasicAttribute(UBasicAttribute _copy)
    {
        if (_copy is null)
            return;

        Attack = _copy.Attack;
        Defense = _copy.Defense;
        MaxHealth = _copy.MaxHealth;
        MovementSpeed = _copy.MovementSpeed;
        AttackSpeed = _copy.AttackSpeed;
    }

    protected override UAttributeBase Add(UAttributeBase _other)
    {
        if (_other is UBasicAttribute other)
        {
            return new UBasicAttribute()
            {
                Attack = Attack + other.Attack,
                Defense = Defense + other.Defense,
                MaxHealth = MaxHealth + other.MaxHealth,
                MovementSpeed = MovementSpeed + other.MovementSpeed,
                AttackSpeed = AttackSpeed + other.AttackSpeed
            };
        }

        return new UBasicAttribute(this);
    }
}
