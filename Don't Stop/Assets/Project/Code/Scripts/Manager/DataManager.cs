using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public interface IDataManager
{
    void LoadData();
    void SaveData();
}

public class DataManager : GenericMonoSingleton<DataManager>, IManager
{
    #region Static

    static string RootSavePath;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init()
    {
        RootSavePath = Path.Combine(Application.persistentDataPath);
    }

    #endregion

    #region API

    public void Save(string _filePath, string _jsonData)
    {
        var savePath = Path.Combine(Application.persistentDataPath, _filePath + ".json");
        File.WriteAllText(savePath, _jsonData, Encoding.Default);
    }

    public void SaveAll()
    {
        var playerState = PlayerState.Get();
        playerState.SaveData();
        playerState.GetInventoryComponent().SaveData();
        playerState.GetEquipmentComponent().SaveData();
    }

    public T LoadJsonData<T>(string _filePath, string _resourcePath) where T : class, new()
    {
        var savePath = Path.Combine(RootSavePath, _filePath + ".json");
        
        T saveData;
        if (File.Exists(savePath))
            saveData = (T) JsonUtility.FromJson(File.ReadAllText(savePath), typeof (T)); // 저장된 파일 로드
        else
        {
            saveData = Resources.Load(_resourcePath) is TextAsset defaultData
                ? (T)JsonUtility.FromJson(defaultData.ToString(), typeof(T))
                : new T();
        }

        return saveData;
    }
    
    #endregion

    #region Interface

    #region IManager

    public void InitManager()
    {
    }

    #endregion
    
    #endregion
}