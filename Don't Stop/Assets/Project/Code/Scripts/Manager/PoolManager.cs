using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    // 에디터에서 설정
    [SerializeField] GameObject[] m_Prefabs;
    
    // 상태
    [SerializeField, ReadOnly] GameObject[] m_Instances;
    PoolInstance[] m_Pools;
    GameObject[] m_InstanceGroups;
    
    // 프로퍼티
    public GameObject[] Prefabs => m_Prefabs;

    // MonoBehaviour
    void Awake()
    {
        m_Pools = new PoolInstance[m_Prefabs.Length];
        m_Instances = new GameObject[m_Prefabs.Length];

        // 버퍼
        GameObject instanceGroup;
        GameObject instance;
        PoolTracker tracker;
        for (int i = 0; i < m_Prefabs.Length; i++)
        {
            // 인스턴스들을 정리해둘 Parent 게임 오브젝트 생성
            instanceGroup = new GameObject(m_Prefabs[i].name + " Pool");
            instanceGroup.transform.parent = transform;

            // PoolTracker를 부착한 프리팹 인스턴스 생성
            instance = Instantiate(m_Prefabs[i], instanceGroup.transform);
            instance.name = m_Prefabs[i].name;
            tracker = instance.AddComponent<PoolTracker>();
            tracker.PrefabID = i;
            instance.SetActive(false);
            m_Instances[i] = instance;

            // Pool 생성
            m_Pools[i] = new PoolInstance(instance, instanceGroup.transform);
        }
    }

    public IObjectPool<GameObject> GetPool(GameObject _prefab)
    {
        for (int i = 0; i < m_Prefabs.Length; i++)
        {
            if (m_Prefabs[i] == _prefab)
                return GetPool(i);
        }

        return null;
    }
    
    public IObjectPool<GameObject> GetPool(int _prefabID)
    {
        return m_Pools.Length > _prefabID ? m_Pools[_prefabID].Get() : null;
    }

    public int GetPrefabID(GameObject _prefab)
    {
        for (int i = 0; i < m_Prefabs.Length; i++)
        {
            if (m_Prefabs[i] == _prefab)
                return i;
        }

        return -1;
    }
}

// Pool Manager에서만 Add 해야함
public class PoolTracker : MonoBehaviour
{
    [SerializeField, ReadOnly] int m_PrefabID = -1;

    public int PrefabID
    {
        get => m_PrefabID;
        set
        {
            if (m_PrefabID < 0)
                m_PrefabID = value;
        }
    }
}

public class PoolInstance
{
    readonly GameObject m_Prefab;
    readonly IObjectPool<GameObject> m_Pool;
    readonly Transform m_InstanceGroup;

    public GameObject Prefab => m_Prefab;
    public IObjectPool<GameObject> Get() => m_Pool;

    public PoolInstance(GameObject _prefab, Transform _instanceGroup, bool _collectionCheck = true, int _defaultCapacity = 10, int _maxSize = 10000)
    {
        // 변수 설정
        m_Prefab = _prefab;
        m_InstanceGroup = _instanceGroup;

        // 오브젝트 풀 생성
        m_Pool = new ObjectPool<GameObject>(OnCreate, OnGet, OnRelease, OnDestroy, _collectionCheck, _defaultCapacity,
            _maxSize);
    }
    
    GameObject OnCreate()
    {
        GameObject instance = UnityEngine.Object.Instantiate(m_Prefab, m_InstanceGroup);
        instance.SetActive(true);
        instance.SetActive(false); // For Awake

        return instance;
    }

    void OnGet(GameObject _instance)
    {
        //_instance.SetActive(true);
        // 선택 옵션으로 바꿀 예정
    }
    
    void OnRelease(GameObject _instance)
    {
        _instance.SetActive(false);
        _instance.transform.parent = m_InstanceGroup;
    }
    
    void OnDestroy(GameObject _instance)
    {
        
    }
}