using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    // 에디터 설정
    [SerializeField] LevelConfig[] m_LevelConfig;

    // 할당
    Transform[] m_SpawnPoints;
    
    // 상태
    int m_StageLevel;

    // 버퍼
    float timer;

    #region Event

    void OnStageStart_Event(int _stageLevel)
    {
        m_StageLevel = Mathf.Min(_stageLevel, m_LevelConfig.Length - 1);
    }

    #endregion

    void Awake()
    {
        m_SpawnPoints = GetComponentsInChildren<Transform>();
    }

    void OnEnable()
    {
        SurvivalGameManager.Get().OnStageStart += OnStageStart_Event;
    }

    void OnDisable()
    {
        SurvivalGameManager.Get().OnStageStart -= OnStageStart_Event;
    }

    void Update()
    {
        // 게임 정지
        if (TimeManager.Get().IsPaused) return;

        timer += Time.deltaTime;

        if (timer > m_LevelConfig[m_StageLevel].SpawnTime)
        {
            foreach (var spawnData in m_LevelConfig[m_StageLevel].EnemyConfig)
            {
                Spawn(spawnData);
            }
            timer = 0;
        }
    }

    void Spawn(SpawnData _spawnData)
    {
        Enemy enemy = PoolManager.GetInstance<Enemy>(_spawnData.Prefab);
        enemy.transform.position = m_SpawnPoints[Random.Range(1, m_SpawnPoints.Length)].position;
        enemy.Init(_spawnData);
        enemy.gameObject.SetActive(true);
    }
    
    [Serializable]
    class LevelConfig
    {
        [SerializeField] float m_SpawnTime;
        public float SpawnTime => m_SpawnTime;

        [SerializeField] int m_MaxSpawnCount;
        public int MaxSpawnCount => m_MaxSpawnCount;

        [SerializeField] SpawnData[] m_EnemyConfig;
        public SpawnData[] EnemyConfig => m_EnemyConfig;
    }
}

[Serializable]
public class SpawnData
{
    [SerializeField] GameObject m_Prefab;
    public GameObject Prefab => m_Prefab;
    
    [SerializeField] int m_Type;
    public int Type => m_Type;

    [SerializeField] int m_Level;
    public int Level => m_Level;

    [SerializeField] float m_Probability;
    public float Probability => m_Probability;
}