using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionController : MonoBehaviour
{
    int level = 0;
    public static ProgressionController Instance { get; private set; }
    public int Level { get => level; set => level = value; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        EnemySpawner.OnLevelOver += EnemySpawner_OnLevelOver;
    }

    private void OnDestroy()
    {
        EnemySpawner.OnLevelOver -= EnemySpawner_OnLevelOver;
    }

    private void EnemySpawner_OnLevelOver()
    {
        level++;
    }

    public void StartNextLevel()
    {       
        if(level < K.NumberOfLevels)
        {
            Debug.Log(K.GameScene + level);
            SceneController.Instance.LoadNextScene(K.GameScene + level);
        }
        else
        {
            SceneController.Instance.LoadNextScene(K.BossScene);
        }
        
    }
}
