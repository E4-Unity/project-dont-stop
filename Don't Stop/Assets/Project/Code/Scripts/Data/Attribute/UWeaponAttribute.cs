using System;
using Framework;

[Serializable]
public class UWeaponAttribute : UAttribute<UWeaponAttribute>
{
    public int Damage;
    public float AttackRange;
    public int ProjectileNum;
    public float AttackRate;

    public static UWeaponAttribute operator +(UWeaponAttribute _left, UWeaponAttribute _right) => new UWeaponAttribute()
    {
        Damage = _left.Damage + _right.Damage,
        AttackRange = _left.AttackRange + _right.AttackRange,
        ProjectileNum = _left.ProjectileNum + _right.ProjectileNum,
        AttackRate = _left.AttackRate + _right.AttackRate
    };

    public override UWeaponAttribute Add(UWeaponAttribute _other) => new UWeaponAttribute()
    {
        Damage = Damage + _other.Damage,
        AttackRange = AttackRange + _other.AttackRange,
        ProjectileNum = ProjectileNum + _other.ProjectileNum,
        AttackRate = AttackRate + _other.AttackRate
    };
}
