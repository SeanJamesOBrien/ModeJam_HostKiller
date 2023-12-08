using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static event Action OnEnemiesDefeated = delegate { };
    public static event Action OnLevelOver = delegate { };
    [SerializeField] SpawnChanceSO[] spawnSettings;
    [SerializeField] List<Enemy> enemyPrefabs = new List<Enemy>();
    Dictionary<int, Enemy> enemies = new Dictionary<int, Enemy>();

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
    int index = 0;

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

    private void Enemy_OnEnemyDestroyed(int enemyId)
    {
        if (!enemies.ContainsKey(enemyId))
        {
            return;
        }
        enemies.Remove(enemyId);
        remainingEnemies--;
        enemyUI.UpdateText(remainingEnemies);
        if (enemies.Count <= minNumEnemies)
        {
            SpawnEnemies(maxEnemiesAtOnce - enemies.Count);
        }
        if(remainingEnemies <= 0)
        {
            OnEnemiesDefeated?.Invoke();
            StartCoroutine(GameOverDelay());   
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
        newEnemy.Id = index;
        enemies.Add(index, newEnemy);
        index++;
    }


    private Enemy GetRandomEnemy()
    {
        return enemyPrefabs[spawnSettings[ProgressionController.Instance.Level].ChooseSpawnedEnemy()];
    }

    private Vector3 GetPosition()
    {
        Vector3 pos = Vector3.zero;
        bool canSpawnHere = false;
        int watchDog = 0;
        while (!canSpawnHere)
        {
            pos = new Vector3(UnityEngine.Random.Range(-groundSize.x, groundSize.x),
                              UnityEngine.Random.Range(-groundSize.y, groundSize.y));

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

    public IEnumerator GameOverDelay()
    {
        float time = 0;
        while (time < K.GameOverDelay)
        {
            time += Time.deltaTime;
            yield return null;
        }
        OnLevelOver?.Invoke();
    }
}