using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<Enemy> enemyPrefabs = new List<Enemy>();
    List<Enemy> enemies = new List<Enemy>();

    [SerializeField] Transform playerPosition;
    [SerializeField] EnemyCountUI enemyUI;

    [Header("Spawn Settings")]
    [SerializeField] LayerMask layerMask;
    [SerializeField] int totalNumberEnemies;
    [SerializeField] int maxEnemiesAtOnce;
    [SerializeField] int minNumEnemies;
    [SerializeField] Transform ground;
    Vector2 groundSize;
    int remainingEnemies = 0;

    void Start()
    {
        groundSize.x = ground.localScale.x / 2;
        groundSize.y = ground.localScale.y / 2; ;
        remainingEnemies = totalNumberEnemies;
        enemyUI.UpdateText(remainingEnemies);
        SpawnEnemies(maxEnemiesAtOnce);

        Enemy.OnEnemyDestroyed += Enemy_OnEnemyDestroyed;
    }

    private void OnDestroy()
    {
        Enemy.OnEnemyDestroyed -= Enemy_OnEnemyDestroyed;
    }

    private void Enemy_OnEnemyDestroyed(Enemy enemy)
    {
        enemies.Remove(enemy);
        remainingEnemies--;
        enemyUI.UpdateText(remainingEnemies);

        if (enemies.Count < minNumEnemies)
        {
            SpawnEnemies(maxEnemiesAtOnce - enemies.Count);
        }
    }

    private void SpawnEnemies(int numEnemies)
    {
        for (int i = 0; i < numEnemies; i++)
        {
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        if (remainingEnemies - enemies.Count <= 0 || 
            enemies.Count > maxEnemiesAtOnce)
        {
            return;
        }
        
        Enemy newEnemy = Instantiate(GetRandomEnemy(),
                                     GetPosition(),
                                     Quaternion.identity,
                                     transform);
        newEnemy.Player = playerPosition;
        enemies.Add(newEnemy);     
    }

    private Enemy GetRandomEnemy()
    {
        return enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
    }

    private Vector3 GetPosition()
    {
        Vector3 pos = Vector3.zero;
        bool canSpawnHere = false;
        int watchDog = 0;
        while (!canSpawnHere)
        {
            pos = new Vector3(Random.Range(-groundSize.x, groundSize.x),
                                 Random.Range(-groundSize.y, groundSize.y));

            canSpawnHere = CheckOverlap(pos);
            if (canSpawnHere)
            {
                break;
            }

            watchDog++;
            if (watchDog > 50)
            {
                Debug.LogWarning("position not found");
                break;
            }
        }
        return pos;
    }

    private bool CheckOverlap(Vector3 pos)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(pos, 3f, layerMask);      
        if (colliders.Length > 0)
        {
            return false;
        }
        return true;
    }
}