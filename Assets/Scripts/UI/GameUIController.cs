using System;
using TMPro;
using UnityEngine;

public class GameUIController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject levelCompletePanel;
    bool isPaused = false;

    private void Start()
    {
        levelText.text = "Level: " + (ProgressionController.Instance.Level + 1);
        isPaused = false;
        Time.timeScale = 1;
        Cursor.visible = isPaused;
        PlayerController.OnPlayerDestroyed += PlayerController_OnPlayerDestroyed;
        EnemySpawner.OnLevelOver += EnemySpawner_OnLevelOver;
    }

    private void OnDestroy()
    {
        PlayerController.OnPlayerDestroyed -= PlayerController_OnPlayerDestroyed; 
        EnemySpawner.OnLevelOver -= EnemySpawner_OnLevelOver;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }       
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pausePanel.SetActive(isPaused);
        Time.timeScale = Convert.ToSingle(!isPaused);
        Cursor.visible = isPaused;
    }

    public void MainMenu()
    {
        SceneController.Instance.LoadNextScene(K.MainMenuScene);
    }

    public void Continue()
    {
        ProgressionController.Instance.StartNextLevel();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void PlayerController_OnPlayerDestroyed()
    {
        Cursor.visible = true;
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(true);
        levelCompletePanel.SetActive(false);
    }

    private void EnemySpawner_OnLevelOver()
    {
        Cursor.visible = true;
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        levelCompletePanel.SetActive(true);
    }
}