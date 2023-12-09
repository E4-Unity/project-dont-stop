using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : E4.Utility.GenericMonoSingleton<PoolManager>
{
    #region Field

    Dictionary<GameObject, int> m_Prefabs = new Dictionary<GameObject, int>();

    List<PoolInstance> m_Pools = new List<PoolInstance>();

    #endregion

    #region State

    [Header("State")]
    [SerializeField, ReadOnly] List<GameObject> m_Instances = new List<GameObject>();

    #endregion

    #region Buffer

    GameObject instanceGroup;
    GameObject instance;
    PoolTracker tracker;
    bool isActive;
    int prefabID;
    PoolInstance poolInstance;

    #endregion

    #region API

    public static T GetInstance<T>(GameObject _prefab)
    {
        return Instance.m_Prefabs.TryGetValue(_prefab, out var id)
            ? Instance.m_Pools[id].GetPool().Get().GetComponent<T>()
            : Instance.CreatePool(_prefab).Get().GetComponent<T>();
    }
    
    public static GameObject GetInstance(GameObject _prefab)
    {
        return Instance.m_Prefabs.TryGetValue(_prefab, out var id)
            ? Instance.m_Pools[id].GetPool().Get()
            : Instance.CreatePool(_prefab).Get();
    }

    public static void ReleaseInstance(GameObject _instance)
    {
        var tracker = _instance.GetComponent<PoolTracker>();
        if (tracker is null)
            Destroy(_instance);
        else
            Instance.m_Pools[tracker.PrefabID].GetPool().Release(_instance);
    }

    #endregion

    #region Method

    IObjectPool<GameObject> CreatePool(GameObject _newPrefab)
    {
        // Pool Manager에 Prefab 등록 및 ID 할당
        prefabID = m_Prefabs.Count;
        m_Prefabs.Add(_newPrefab, prefabID);
        
        // 생성된 오브젝트들을 담아둘 빈 오브젝트
        instanceGroup = new GameObject(_newPrefab.name + " Pool")
        {
            transform =
            {
                parent = transform
            }
        };
        
        // 비활성화된 Prefab 인스턴스 생성
        isActive = _newPrefab.activeSelf;
        _newPrefab.SetActive(false);
        instance = Instantiate(_newPrefab, instanceGroup.transform);
        instance.name = _newPrefab.name;
        _newPrefab.SetActive(isActive);

        // Prefab 인스턴스에 PoolTracker 부착 및 인스턴스 리스트에 등록
        tracker = instance.AddComponent<PoolTracker>();
        tracker.PrefabID = prefabID;
        m_Instances.Add(instance);

        // Pool 생성 및 등록
        poolInstance = new PoolInstance(instance, instanceGroup.transform);
        m_Pools.Add(poolInstance);

        return poolInstance.GetPool();
    }

    #endregion
}

// Pool Manager에서만 추가
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
    readonly GameObject m_Prefab; // 복제 대상
    readonly IObjectPool<GameObject> m_Pool; // 실제 오브젝트 풀
    readonly Transform m_InstanceGroup; // 생성된 오브젝트들을 담아두는 빈 오브젝트

    #region Buffer

    GameObject instance;

    #endregion

    public IObjectPool<GameObject> GetPool() => m_Pool;

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
        instance = Object.Instantiate(m_Prefab, m_InstanceGroup);

        return instance;
    }

    void OnGet(GameObject _instance)
    {
        
    }
    
    void OnRelease(GameObject _instance)
    {
        _instance.SetActive(false);
        // _instance.transform.parent = m_InstanceGroup;
    }
    
    void OnDestroy(GameObject _instance)
    {
        
    }
}