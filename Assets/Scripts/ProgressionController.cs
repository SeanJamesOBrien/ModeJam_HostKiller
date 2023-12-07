using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressionController : MonoBehaviour
{
    int level;
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
    }

    public void StartNextLevel()
    {
        level++;
        SceneController.Instance.LoadNextScene("GameScene" + level);
    }
}
