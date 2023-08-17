using System;
using Framework;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Player Definition", menuName = "Scriptable Object/Attribute Definition/Player")]
public class PlayerDefinition : EntityDefinition<PlayerDefinition, UBasicAttribute>
{
    public override UAttributeDataBase GetAttributeDataBase()
    {
        return new UPlayerData()
        {
            Definition = this,
            Level = 0
        };
    }
}
