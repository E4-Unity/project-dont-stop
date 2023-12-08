using UnityEngine;
using Random = UnityEngine.Random;

public class EnforceButton : MonoBehaviour
{
    /* 컴포넌트 */
    PlayerState m_PlayerState;
    PlayerInventory m_PlayerInventory;

    /* MonoBehaviour */
    void Awake()
    {
        m_PlayerState = GlobalGameManager.Instance.GetPlayerState();
        m_PlayerInventory = m_PlayerState.GetInventoryComponent();
    }

    public void EnforceCharacter()
    {
        UCharacterData characterData = m_PlayerState.CharacterData;
        
        if (characterData.NextGold <= m_PlayerInventory.Gold)
        {
            m_PlayerInventory.Gold -= characterData.NextGold;
            characterData.Exp += Mathf.RoundToInt(characterData.NextExp * Random.Range(0.1f, 1f));
            m_PlayerState.SaveData();
        }
    }
    
    // Enforce Weapon
    
    // Enforce Gear
}
