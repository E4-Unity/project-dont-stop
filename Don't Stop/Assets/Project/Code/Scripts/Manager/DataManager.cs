using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class DataManager : GenericMonoSingleton<DataManager>, IManager
{
    #region Static

    static string SavePath;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init()
    {
        SavePath = Path.Combine(Application.persistentDataPath, "PlayerData.json");
    }

    #endregion
    
    #region Field

    // 모든 정보
    JsonPlayerData m_SaveData;
    [SerializeField] UPlayerData m_PlayerData = new UPlayerData();
    [SerializeField] List<UCharacterData> m_CharacterDataList = new List<UCharacterData>();
    [SerializeField] List<UWeaponData> m_WeaponDataList = new List<UWeaponData>();
    [SerializeField] List<UGearData> m_GearDataList = new List<UGearData>();

    #endregion

    #region Property

    public UPlayerData GetPlayerData() => m_PlayerData;
    public UCharacterData GetCharacterData(int _id) => m_CharacterDataList.Find(_e => _e.DefinitionID == _id);
    public UWeaponData GetWeaponData(int _id) => m_WeaponDataList.Find(_e => _e.DefinitionID == _id);

    public UGearData GetGearData(int _id) => m_GearDataList.Find(_e => _e.DefinitionID == _id);

    #endregion

    public void SaveJsonData()
    {
        JsonPlayerData jsonSaveData = new JsonPlayerData();
        
        m_PlayerData.GetSaveData(out jsonSaveData.PlayerData);

        foreach (var data in m_CharacterDataList)
        {
            var saveData = new FSavedAttributeData();
            data.GetSaveData(out saveData);
            jsonSaveData.CharacterData.Add(saveData);
        }
        
        foreach (var data in m_WeaponDataList)
        {
            var saveData = new FSavedAttributeData();
            data.GetSaveData(out saveData);
            jsonSaveData.WeaponData.Add(saveData);
        }
        
        foreach (var data in m_GearDataList)
        {
            var saveData = new FSavedAttributeData();
            data.GetSaveData(out saveData);
            jsonSaveData.GearData.Add(saveData);
        }

        jsonSaveData.CharacterID = PlayerState.Get().CharacterData.DefinitionID;

        // 장착된 장비 정보 저장
        var equipmentComponent = PlayerState.Get().GetEquipmentComponent();
        jsonSaveData.WeaponID = equipmentComponent.WeaponData.DefinitionID;
        jsonSaveData.GearIDList = equipmentComponent.GetGearIDList();

        // 골드 및 크리스탈 정보 저장
        var inventory = PlayerState.Get().GetInventoryComponent();
        jsonSaveData.Gold = inventory.Gold;
        jsonSaveData.Crystal = inventory.Crystal;
        
        // JSON 파일로 저장
        var jsonData = JsonUtility.ToJson(jsonSaveData);
        File.WriteAllText(SavePath, jsonData, Encoding.Default);
    }

    void LoadJsonData()
    {
        if (File.Exists(SavePath))
            m_SaveData = JsonUtility.FromJson<JsonPlayerData>(File.ReadAllText(SavePath));
        else
        {
            m_SaveData = new JsonPlayerData();

            for (int i = 0; i < 4; i++)
            {
                var characterData = new UCharacterData();
                var data = new FSavedAttributeData
                {
                    DefinitionID = i
                };
                characterData.Init(data);
                m_CharacterDataList.Add(characterData);
            }
            
            var newWeaponData = new UWeaponData();
            newWeaponData.Init(new FSavedAttributeData());
            m_WeaponDataList.Add(newWeaponData);
        }

        // 데이터 로딩
        m_PlayerData.Init(m_SaveData.PlayerData);
        m_PlayerData.PlayerName = "Player Unknown";

        foreach (var characterData in m_SaveData.CharacterData)
        {
            var newCharacterData = new UCharacterData();
            newCharacterData.Init(characterData);
            m_CharacterDataList.Add(newCharacterData);
        }
        
        foreach (var weaponData in m_SaveData.WeaponData)
        {
            var newWeaponData = new UWeaponData();
            newWeaponData.Init(weaponData);
            m_WeaponDataList.Add(newWeaponData);
        }
        
        foreach (var gearData in m_SaveData.GearData)
        {
            var newGearData = new UGearData();
            newGearData.Init(gearData);
            m_GearDataList.Add(newGearData);
        }
    }

    void UpdatePlayerState()
    {
        // Player State 초기화
        PlayerState.Get().PlayerData = m_PlayerData;
        PlayerState.Get().SelectCharacter(m_SaveData.CharacterID, false);
        
        // Equipment Component 초기화
        var equipmentComponent = PlayerState.Get().GetEquipmentComponent();
        equipmentComponent.AddWeapon(m_SaveData.WeaponID);
        foreach (var gearID in m_SaveData.GearIDList)
        {
            equipmentComponent.AddGear(gearID);
        }
        equipmentComponent.FinishInit();
        
        // 골드 및 크리스탈 정보 로드
        var inventory = PlayerState.Get().GetInventoryComponent();
        inventory.Gold = m_SaveData.Gold;
        inventory.Crystal = m_SaveData.Crystal;

        PlayerState.Get().GetStatsComponent().Init(equipmentComponent);

        PlayerState.Get().FinishInit();
    }

    #region Interface

    protected override void Awake()
    {
        base.Awake();
        LoadJsonData();
    }

    public void InitManager()
    {
        UpdatePlayerState();
    }

    #endregion
}

[Serializable]
public class JsonPlayerData
{
    public FSavedAttributeData PlayerData = new FSavedAttributeData();
    public List<FSavedAttributeData> CharacterData = new List<FSavedAttributeData>();
    public List<FSavedAttributeData> WeaponData = new List<FSavedAttributeData>();
    public List<FSavedAttributeData> GearData = new List<FSavedAttributeData>();
    public int CharacterID = 0;
    public int WeaponID = 0;
    public List<int> GearIDList = new List<int>();
    public int Gold = 0;
    public int Crystal = 0;
}