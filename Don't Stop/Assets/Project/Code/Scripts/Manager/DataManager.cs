using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : GenericMonoSingleton<DataManager>
{
    #region Field

    // 모든 정보
    [SerializeField] UPlayerData m_PlayerData = new UPlayerData();
    [SerializeField] List<UCharacterData> m_CharacterDataList = new List<UCharacterData>();
    [SerializeField] List<UWeaponData> m_WeaponDataList = new List<UWeaponData>();
    [SerializeField] List<UGearData> m_GearDataList = new List<UGearData>();

    string m_SavePath;

    #endregion

    #region Property

    public UPlayerData GetPlayerData() => m_PlayerData;
    public UCharacterData GetCharacterData(int _id) => m_CharacterDataList.Find(_e => _e.DefinitionID == _id);
    public UWeaponData GetWeaponData(int _id) => m_WeaponDataList.Find(_e => _e.DefinitionID == _id);

    public UGearData GetGearData(int _id) => m_GearDataList.Find(_e => _e.DefinitionID == _id);

    #endregion

    protected override void Awake_Implementation()
    {
        base.Awake_Implementation();
        PlayerDefinition.Definitions = Resources.LoadAll<PlayerDefinition>("Data/PlayerDefinition");
        CharacterDefinition.Definitions = Resources.LoadAll<CharacterDefinition>("Data/CharacterDefinition");
        GearDefinition.Definitions = Resources.LoadAll<GearDefinition>("Data/GearDefinition");
        WeaponDefinition.Definitions = Resources.LoadAll<WeaponDefinition>("Data/WeaponDefinition");

        m_SavePath = Application.dataPath + "/Save/PlayerData.json";
    }

    void Start()
    {
        LoadJsonData();
    }

    void SaveJsonData()
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

        var equipmentComponent = PlayerState.Get().GetEquipmentComponent();
        jsonSaveData.WeaponID = equipmentComponent.WeaponData.DefinitionID;
        jsonSaveData.GearIDList = equipmentComponent.GetGearIDList();

        var jsonData = JsonUtility.ToJson(jsonSaveData);
        File.WriteAllText(m_SavePath, jsonData);
    }

    void LoadJsonData()
    {
        var data = JsonUtility.FromJson<JsonPlayerData>(File.ReadAllText(m_SavePath));

        // 데이터 로딩
        m_PlayerData.Init(data.PlayerData);
        m_PlayerData.PlayerName = "Player Unknown";

        foreach (var characterData in data.CharacterData)
        {
            var newCharacterData = new UCharacterData();
            newCharacterData.Init(characterData);
            m_CharacterDataList.Add(newCharacterData);
        }
        
        foreach (var weaponData in data.WeaponData)
        {
            var newWeaponData = new UWeaponData();
            newWeaponData.Init(weaponData);
            m_WeaponDataList.Add(newWeaponData);
        }
        
        foreach (var gearData in data.GearData)
        {
            var newGearData = new UGearData();
            newGearData.Init(gearData);
            m_GearDataList.Add(newGearData);
        }

        // Player State 초기화
        PlayerState.Get().PlayerData = m_PlayerData;
        PlayerState.Get().SelectCharacter(data.CharacterID);
        
        // Equipment Component 초기화
        var equipmentComponent = PlayerState.Get().GetEquipmentComponent();
        equipmentComponent.AddWeapon(data.WeaponID);
        foreach (var gearID in data.GearIDList)
        {
            equipmentComponent.AddGear(gearID);
        }
        equipmentComponent.FinishInit();

        PlayerState.Get().GetStatsComponent().Init(equipmentComponent.GearSlots);

        PlayerState.Get().FinishInit();
    }
}

[Serializable]
public class JsonPlayerData
{
    public FSavedAttributeData PlayerData;
    public List<FSavedAttributeData> CharacterData;
    public List<FSavedAttributeData> WeaponData;
    public List<FSavedAttributeData> GearData;
    public int CharacterID;
    public int WeaponID;
    public List<int> GearIDList;
}