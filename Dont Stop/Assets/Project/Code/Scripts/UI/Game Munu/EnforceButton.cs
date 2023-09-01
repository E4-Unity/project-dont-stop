using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnforceButton : MonoBehaviour
{
    public void EnforceCharacter()
    {
        PlayerInventory inventory = PlayerState.Get().GetInventoryComponent();
        UCharacterData characterData = PlayerState.Get().CharacterData;
        
        if (characterData.NextGold <= inventory.Gold)
        {
            inventory.Gold -= characterData.NextGold;
            characterData.Exp += Mathf.RoundToInt(characterData.NextExp * Random.Range(0.1f, 1f));
            PlayerState.Get().SaveData();
        }
    }
    
    // Enforce Weapon
    
    // Enforce Gear
}
