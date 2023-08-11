using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManagerBoss : MonoBehaviour
{
    static SpawnManagerBoss instance;
    public static SpawnManagerBoss Get() => instance;

    [SerializeField] GameObject[] itemPrefab;
    [SerializeField] GameObject[] enemyPrefab;
    [SerializeField] GameObject HpBar;

    [SerializeField] List<Transform> spawnPoints;
    int currentIndex = 0;
    
    void Start()
    {
        spawnPoints = new List<Transform>();

        foreach (Transform child in transform)
        {
            spawnPoints.Add(child);
        }

        StartCoroutine(SpawnElite());
    }

    IEnumerator SpawnElite()
    {
        if (currentIndex > 9)
        {
            StopCoroutine(SpawnElite());
            GameManagerBoss.Get().spawnEnd = true;
        }

        if(currentIndex <= 9)
        {
            Transform spawnPoint = spawnPoints[currentIndex];

            if(spawnPoint.CompareTag("ItemSpawnPoint"))
            {
                Instantiate(itemPrefab[Random.Range(0,5)], spawnPoint.position, Quaternion.identity);
            }
            else if(spawnPoint.CompareTag("EnemySpawnPoint"))
            {
                Instantiate(enemyPrefab[Random.Range(0,3)], spawnPoint.position, Quaternion.identity);
            }

            currentIndex += 1;
        }
        yield return null;
        StartCoroutine(SpawnElite());
    }
}
