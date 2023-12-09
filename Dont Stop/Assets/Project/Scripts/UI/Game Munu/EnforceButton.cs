using UnityEngine;
using Random = UnityEngine.Random;

public class EnforceButton : MonoBehaviour
{
    /* 컴포넌트 */
    PlayerState m_PlayerState;
    PlayerInventory m_PlayerInventory;
    UISoundManager m_SoundManager;

    /* MonoBehaviour */
    void Awake()
    {
        // 컴포넌트 할당
        var globalGameManager = GlobalGameManager.Instance;
        m_PlayerState = globalGameManager.GetPlayerState();
        m_SoundManager = globalGameManager.GetUISoundManager();
        
        m_PlayerInventory = m_PlayerState.GetInventoryComponent();
    }

    public void EnforceCharacter()
    {
        // 현재 캐릭터 정보 불러오기
        UCharacterData characterData = m_PlayerState.CharacterData;
        
        // 캐릭터 강화
        if (characterData.NextGold <= m_PlayerInventory.Gold)
        {
            m_PlayerInventory.Gold -= characterData.NextGold;
            characterData.Exp += Mathf.RoundToInt(characterData.NextExp * Random.Range(0.1f, 1f));
            m_PlayerState.SaveData();
            
            // 선택 사운드 재생
            m_SoundManager.PlaySound(EUISoundType.SelectButton);
        }
    }
    
    // Enforce Weapon
    
    // Enforce Gear
}
