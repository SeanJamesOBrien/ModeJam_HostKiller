using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class GameUIController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI victoryText;  
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject levelCompletePanel;
    
    [Header("Countdown")]
    [SerializeField] GameObject countdownPanel;
    [SerializeField] TextMeshProUGUI countdownText;
    [SerializeField] float countdownDuration;
    bool isPaused = false;

    private void Start()
    {
        int level = ProgressionController.Instance.Level + 1;
        if (level < 6)
        {
            levelText.text = "Level: " + level;
        }
        else
        {
            levelText.text = "Boss Level";
        }
        
        isPaused = false;
        Cursor.visible = isPaused;
        PlayerController.OnPlayerDestroyedComplete += PlayerController_OnPlayerDestroyedComplete;
        EnemySpawner.OnLevelOver += EnemySpawner_OnLevelOver;
        StartCoroutine(Countdown());   
    }

    private void OnDestroy()
    {
        PlayerController.OnPlayerDestroyedComplete -= PlayerController_OnPlayerDestroyedComplete; 
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

    private void PlayerController_OnPlayerDestroyedComplete()
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
        victoryText.text = "Wave " + ProgressionController.Instance.Level + " complete";
    }

    IEnumerator Countdown()
    {
        Time.timeScale = 0;
        countdownPanel.SetActive(true);
        float time = countdownDuration;
        while (time >= 0)
        {
            countdownText.text = time.ToString("#");
            time -= Time.unscaledDeltaTime;        
            yield return null;
        }
        countdownPanel.SetActive(false);
        Time.timeScale = 1;
    }
}