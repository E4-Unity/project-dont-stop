using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Character Definition", menuName = "Scriptable Object/Attribute Definition/Character")]
public class CharacterDefinition : EntityDefinition<CharacterDefinition, UBasicAttribute>
{
    [SerializeField] int m_BaseWeaponID;
}
