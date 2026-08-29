using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;

    [Header("Spawn Settings")]
    public float minSpawnInterval = 0.5f;
    public float maxSpawnInterval = 2f;

    [Header("Spawn Area")]
    public float minX = -5f;
    public float maxX = 5f;
    public float spawnY = 5f;

    private bool spawning = true;

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (spawning)
        {
            SpawnEnemy();

            float waitTime = Random.Range(
                minSpawnInterval,
                maxSpawnInterval
            );

            yield return new WaitForSeconds(waitTime);
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefabs.Length == 0)
            return;

        int index = Random.Range(0, enemyPrefabs.Length);

        float x = Random.Range(minX, maxX);

        Vector3 spawnPosition = new Vector3(
            x,
            spawnY,
            0f
        );

        Instantiate(
            enemyPrefabs[index],
            spawnPosition,
            Quaternion.identity
        );
    }

    public void StopSpawning()
    {
        spawning = false;
    }
}
