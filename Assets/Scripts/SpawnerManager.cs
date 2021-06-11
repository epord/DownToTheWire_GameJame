using System.Collections;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    // Top to bottom
    public EnemySpawner[] leftSpawners;
    // Top to bottom
    public EnemySpawner[] rightSpawners;
    // Right to left
    public EnemySpawner[] topSpawners;

    public Enemy redEnemyPrefab;
    public Enemy blueEnemyPrefab;
    public Enemy greenEnemyPrefab;
    public Enemy goldEnemyPrefab;

    void Start()
    {
        StartCoroutine(SpawnInStages());
    }

    IEnumerator SpawnInStages()
    {
        // == TUTORIAL ==

        // one red top - center
        Stage1();

        // two red top-center
        yield return new WaitForSeconds(42);
        Stage2();

        // green top-left and red top-right
        yield return new WaitForSeconds(42);
        Stage3();

        // green left-top
        yield return new WaitForSeconds(42);
        Stage4();

        // blue right-top
        yield return new WaitForSeconds(38);
        Stage5();

        // blue top-right
        yield return new WaitForSeconds(38);
        Stage6();

        // gold right bottom
        yield return new WaitForSeconds(38);
        Stage7();

        // red left-top
        yield return new WaitForSeconds(38);
        Stage8();

        // red left-center and green top-right
        yield return new WaitForSeconds(38);
        Stage9();

        // == END TUTORIAL ==

        // start automatic spawn
        StartCoroutine(SpawnAutomatically());
    }


    // ========================================
    // == Hardcoded: 7 left, 7 right, 12 top ==
    // ========================================

    private void Stage1()
    {
        EnemySpawner spawner = topSpawners[5];
        spawner.SpawnEnemy(redEnemyPrefab);
    }

    private void Stage2()
    {
        EnemySpawner spawner = topSpawners[4];
        spawner.SpawnEnemy(redEnemyPrefab);

        spawner = topSpawners[8];
        spawner.SpawnEnemy(redEnemyPrefab);
    }

    private void Stage3()
    {
        EnemySpawner spawner = topSpawners[2];
        spawner.SpawnEnemy(greenEnemyPrefab);

        spawner = topSpawners[8];
        spawner.SpawnEnemy(redEnemyPrefab);
    }

    private void Stage4()
    {
        EnemySpawner spawner = leftSpawners[2];
        spawner.SpawnEnemy(greenEnemyPrefab);
    }

    private void Stage5()
    {
        EnemySpawner spawner = rightSpawners[3];
        spawner.SpawnEnemy(blueEnemyPrefab);
    }

    private void Stage6()
    {
        EnemySpawner spawner = topSpawners[11];
        spawner.SpawnEnemy(blueEnemyPrefab);
    }

    private void Stage7()
    {
        EnemySpawner spawner = rightSpawners[6];
        spawner.SpawnEnemy(goldEnemyPrefab);
    }

    private void Stage8()
    {
        EnemySpawner spawner = leftSpawners[1];
        spawner.SpawnEnemy(redEnemyPrefab);
    }

    private void Stage9()
    {
        EnemySpawner spawner = leftSpawners[3];
        spawner.SpawnEnemy(redEnemyPrefab);

        spawner = topSpawners[3];
        spawner.SpawnEnemy(greenEnemyPrefab);
    }

    private EnemySpawner GetRandomEnemySpawner()
    {
        int listSelector = Random.Range(1, 4);
        if (listSelector == 1) return leftSpawners[Random.Range(0, leftSpawners.Length)];
        else if (listSelector == 2) return rightSpawners[Random.Range(0, rightSpawners.Length)];
        else return topSpawners[Random.Range(0, topSpawners.Length)];
    }

    private Enemy GetRandomEnemyPrefab()
    {
        int listSelector = Random.Range(1, 5);
        if (listSelector == 1) return redEnemyPrefab;
        else if (listSelector == 2) return greenEnemyPrefab;
        else if (listSelector == 3) return goldEnemyPrefab;
        else return blueEnemyPrefab;
    }

    private IEnumerator SpawnAutomatically()
    {
        float automaticModeStartTime = Time.time;
        while (true)
        {
            if (Time.time - automaticModeStartTime < 120)
            {
                yield return new WaitForSeconds(Random.Range(38, 42));
            }
            else if (Time.time - automaticModeStartTime < 220)
            {
                yield return new WaitForSeconds(Random.Range(33, 38));
            } else
            {
                yield return new WaitForSeconds(Random.Range(28, 33));
            }

            int max = 2;
            if (Time.time - automaticModeStartTime > 120)
            {
                max = 3; // Increase after 5 minutes
            }
            if (Time.time - automaticModeStartTime > 220)
            {
                max = 4; // Increase after 10 minutes
            }
            int spawnAmount = Random.Range(1, max);

            for (int i = 0; i < spawnAmount; ++i)
            {
                EnemySpawner spawner = GetRandomEnemySpawner();
                Enemy enemyPrefab = GetRandomEnemyPrefab();
                spawner.SpawnEnemy(enemyPrefab);
            }
        }
    }
}