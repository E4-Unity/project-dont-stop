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

    // 버퍼
    float timer;
    int level;

    void Awake()
    {
        m_SpawnPoints = GetComponentsInChildren<Transform>();
    }

    void Update()
    {
        // 게임 정지
        if (GameManager.Get().IsPaused) return;

        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(GameManager.Get().PlayTime / 10f), m_LevelConfig.Length - 1);

        if (timer > m_LevelConfig[level].SpawnTime)
        {
            foreach (var item in m_LevelConfig[level].EnemyConfig)
            {
                Spawn(item);
            }
            timer = 0;
        }
    }

    void Spawn(SpawnData _spawnData)
    {
        GameObject enemy = GameManager.Get().GetPoolManager().GetPool(_spawnData.Prefab).Get(); // 나중에 캐싱
        enemy.transform.position = m_SpawnPoints[Random.Range(1, m_SpawnPoints.Length)].position;
        enemy.GetComponent<Enemy>().Init(_spawnData);
        enemy.SetActive(true);
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