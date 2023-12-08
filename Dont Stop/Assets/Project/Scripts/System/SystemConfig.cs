using System;
using System.Collections.Generic;using UnityEditor;
using UnityEngine;

public enum EManagerClass
{
    PlayerState,
    PoolManager,
    TimeManager
}

public class ManagerClassDictionary
{
    static Dictionary<EManagerClass, Type> ManagerDict = new Dictionary<EManagerClass, Type>()
    {
        { EManagerClass.PlayerState, typeof(PlayerState) },
        { EManagerClass.PoolManager, typeof(PoolManager) },
        { EManagerClass.TimeManager, typeof(TimeManager) }
    };

    public static Type[] Get(EManagerClass[] _managerClasses)
    {
        Type[] managerClassList = new Type[_managerClasses.Length];
        for (int i = 0; i < _managerClasses.Length; i++)
        {
            managerClassList[i] = ManagerDict[_managerClasses[i]];
        }

        return managerClassList;
    }
}

[CreateAssetMenu(menuName = "Scriptable Object/Config/System", fileName = "System Config")]
public class SystemConfig : ScriptableObject
{
    // 매니저 자동 생성 리스트
    [SerializeField] EManagerClass[] m_ManagerClassList;

    public Type[] ManagerClassList => ManagerClassDictionary.Get(m_ManagerClassList);

    [SerializeField] GameObject[] m_ManagerPrefabList;

    public GameObject[] ManagerPrefabList => m_ManagerPrefabList;
}
