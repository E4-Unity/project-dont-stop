using System;
using System.Collections.Generic;
using UnityEngine;

public class SystemManager
{
    static SystemConfig Config;
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnBeforeSceneLoaded()
    {
        LoadSystemConfig();
        LoadDefinitions();
        CreateManagers();
    }

    static void LoadSystemConfig()
    {
        Debug.Log("SystemManager > Load System Config");
        Config = Resources.Load<SystemConfig>("Config/System Config");
    }

    static void LoadDefinitions()
    {
        PlayerDefinition.Definitions = Resources.LoadAll<PlayerDefinition>("Data/PlayerDefinition");
        CharacterDefinition.Definitions = Resources.LoadAll<CharacterDefinition>("Data/CharacterDefinition");
        GearDefinition.Definitions = Resources.LoadAll<GearDefinition>("Data/GearDefinition");
        WeaponDefinition.Definitions = Resources.LoadAll<WeaponDefinition>("Data/WeaponDefinition");
        
        Debug.Log("SystemManager > Load Definitions");
    }

    static void CreateManagers()
    {
        List<IManager> managers = new List<IManager>(Config.ManagerClassList.Length + Config.ManagerPrefabList.Length);

        // 매니저 오브젝트 생성
        Debug.Log("SystemManager > Create Managers");
        
        // 매니저 생성
        managers.AddRange(ManagerFactory.CreateManager(Config.ManagerClassList, false));
        managers.AddRange(ManagerFactory.CreateManager(Config.ManagerPrefabList, false));

        // 매니저 초기화
        Debug.Log("SystemManager > Init Managers");
        foreach (var manager in managers)
        {
            manager?.InitManager();
        }

    }
}
