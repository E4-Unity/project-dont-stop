using System;
using Framework;

[Serializable]
public class UWeaponAttribute : UAttribute<UWeaponAttribute>
{
    public int Damage;
    public float AttackRange;
    public int ProjectileNum;
    public float AttackRate;
    public int Penetration;
    public float Speed;

    public UWeaponAttribute() {}

    public UWeaponAttribute(UWeaponAttribute _copy)
    {
        if (_copy is null)
            return;

        Damage = _copy.Damage;
        AttackRange = _copy.AttackRange;
        ProjectileNum = _copy.ProjectileNum;
        AttackRate = _copy.AttackRate;
        Penetration = _copy.Penetration;
        Speed = _copy.Speed;
    }

    protected override UAttributeBase Add(UAttributeBase _other)
    {
        if (_other is UWeaponAttribute other)
        {
            return new UWeaponAttribute()
            {
                Damage = Damage + other.Damage,
                AttackRange = AttackRange + other.AttackRange,
                ProjectileNum = ProjectileNum + other.ProjectileNum,
                AttackRate = AttackRate + other.AttackRate,
                Penetration = Penetration + other.Penetration,
                Speed = Speed + other.Speed
            };
        }

        return new UWeaponAttribute(this);
    }
}
