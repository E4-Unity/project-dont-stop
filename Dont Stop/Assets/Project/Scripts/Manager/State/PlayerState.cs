using System;
using System.Collections.Generic;
using E4.Utility;
using UnityEngine;

[Serializable]
public class PlayerStateSaveData
{
    public FSavedAttributeData PlayerData = new FSavedAttributeData();
    public List<FSavedAttributeData> CharacterDataList = new List<FSavedAttributeData>();
    public int CharacterID = 0;
}

[RequireComponent(typeof(PlayerInventory), typeof(PlayerEquipment), typeof(PlayerStats))]
public class PlayerState : GenericMonoSingleton<PlayerState>, IManager, ISavable<PlayerStateSaveData>
{
    /* 컴포넌트 */
    
    PlayerInventory m_InventoryComponent;
    PlayerEquipment m_EquipmentComponent;
    PlayerStats m_StatsComponent;

    public PlayerInventory GetInventoryComponent() => m_InventoryComponent;

    public PlayerEquipment GetEquipmentComponent() => m_EquipmentComponent;

    public PlayerStats GetStatsComponent() => m_StatsComponent;
    
    /* 필드 */
    
    [SerializeField] UPlayerData m_PlayerData;
    [SerializeField] UCharacterData m_SelectedCharacterData;
    [SerializeField] List<UCharacterData> m_CharacterDataList = new List<UCharacterData>();
    
    // ISavable
    PlayerStateSaveData saveData;

    /* 프로퍼티 */

    public UPlayerData PlayerData
    {
        get => m_PlayerData;
        set => m_PlayerData = value;
    }

    public event Action<UCharacterData> OnCharacterDataUpdate;
    public UCharacterData CharacterData
    {
        get => m_SelectedCharacterData;
        set
        {
            m_SelectedCharacterData = value;
            OnCharacterDataUpdate?.Invoke(value);
        }
    }

    /* API */

    public void SelectCharacter(int _id)
    {
        var characterData = GetCharacterData(_id);
        if (characterData is null)
        {
            print("Selected Character is not exist in Data Manager : " + _id);
            return;
        }
        
        CharacterData = GetCharacterData(_id);

        // TODO 리팩토링
        if (saveData.CharacterID != _id)
        {
            SaveData();
        }
    }

    /* 메서드 */

    UCharacterData GetCharacterData(int _id)
    {
        UCharacterData characterData = m_CharacterDataList.Find(_e => _e.Definition.ID == _id);
        if (characterData is null)
        {
            characterData = new UCharacterData();
            var data = new FSavedAttributeData()
            {
                DefinitionID = _id
            };
            characterData.Init(data);
            m_CharacterDataList.Add(characterData);
        }

        return characterData;
    }

    /* MonoBehaviour */

    protected override void Awake()
    {
        base.Awake();
        m_InventoryComponent = GetComponent<PlayerInventory>();
        m_EquipmentComponent = GetComponent<PlayerEquipment>();
        m_StatsComponent = GetComponent<PlayerStats>();
        
        m_InventoryComponent.Init(GetEquipmentComponent());
        m_EquipmentComponent.Init(GetInventoryComponent());
    }

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }

    /* IManager 인터페이스 */
    public void InitManager()
    {
        LoadData();
        GetInventoryComponent().LoadData();
        GetEquipmentComponent().LoadData();
        GetStatsComponent().Init(GetEquipmentComponent());
    }

    /* IDataManager 인터페이스 */

    public void LoadData()
    {
        saveData = DataManager.LoadData(this) ?? new PlayerStateSaveData();

        // Player Data 로드
        m_PlayerData = new UPlayerData()
        {
            PlayerName = "Player"
        };
        m_PlayerData.Init(saveData.PlayerData);

        // Character Data 로드
        foreach (var characterData in saveData.CharacterDataList)
        {
            var newCharacterData = new UCharacterData();
            newCharacterData.Init(characterData);
            m_CharacterDataList.Add(newCharacterData);
        }
        
        // Selected Character Data 로드
        SelectCharacter(saveData.CharacterID);
    }

    public void SaveData()
    {
        // 데이터 저장 요청
        if (!DataManager.RequestSaveData(this)) return;
        
        // Player Data 저장
        PlayerStateSaveData newSaveData = new PlayerStateSaveData
        {
            PlayerData = m_PlayerData.GetSaveData()
        };

        // Character Data 저장
        foreach (var characterData in m_CharacterDataList)
        {
            newSaveData.CharacterDataList.Add(characterData.GetSaveData());
        }
        
        // 선택된 캐릭터 저장
        newSaveData.CharacterID = m_SelectedCharacterData.GetSaveData().DefinitionID;

        // 세이브 데이터 저장
        saveData = newSaveData;
    }

    /* ISavable 인터페이스 */
    public PlayerStateSaveData Data => saveData;

    public bool IsDirty { get; set; }
}