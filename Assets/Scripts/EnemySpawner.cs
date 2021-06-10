using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float minTimeBetweenSpawns = 10f;
    public float maxTimeBetweenSpawns = 15f;
    public float enemySpeed = 0.12f;

    public Direction spawnDirection = Direction.RIGHT;

    public float redFrequency = 1f;
    public float blueFrequency = 1f;
    public float greenFrequency = 1f;
    public float goldFrequency = 1f;

    public Enemy redEnemyPrefab;
    public Enemy blueEnemyPrefab;
    public Enemy greenEnemyPrefab;
    public Enemy goldEnemyPrefab;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(SpawnCoroutine());
    }

    IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            Enemy nextEnemyPrefab;
            float colorRand = Random.Range(0, redFrequency + blueFrequency + greenFrequency + goldFrequency);
            if (colorRand < redFrequency)
            {
                nextEnemyPrefab = redEnemyPrefab;
            } else if (colorRand < redFrequency + blueFrequency)
            {
                nextEnemyPrefab = blueEnemyPrefab;
            } else if (colorRand < redFrequency + blueFrequency + greenFrequency)
            {
                nextEnemyPrefab = greenEnemyPrefab;
            } else
            {
                nextEnemyPrefab = goldEnemyPrefab;
            }

            float nextSpawnTime = Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns);
            Debug.Log(nextSpawnTime);
            yield return new WaitForSeconds(nextSpawnTime);
            
            SpawnEnemy(nextEnemyPrefab);
        }
    }

    public void SpawnEnemy(Enemy enemyPrefab)
    {
        Enemy enemy = Instantiate(enemyPrefab, transform.position, transform.rotation);
        enemy.speed = enemySpeed;
        enemy.movingDirection = spawnDirection;
    }
}
