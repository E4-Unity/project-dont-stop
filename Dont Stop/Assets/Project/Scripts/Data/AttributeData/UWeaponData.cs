using System;
using Framework;
using UnityEngine;

[Serializable]
public class UWeaponData : UAttributeData<WeaponDefinition, UWeaponAttribute>
{
    public EWeaponType WeaponType => Definition.WeaponType;

    #region Definition Property

    public GameObject Prefab => Definition.Prefab;
    public Sprite DisplaySprite => Definition.DisplaySprite;
    public Sprite BulletSprite => Definition.BulletSprite;

    #endregion
}