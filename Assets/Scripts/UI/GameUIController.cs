using System;
using UnityEngine;

public class GameUIController : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject gameOverPanel;
    bool isPaused = false;

    [SerializeField] GameObject lastPlayerHealth;
    [SerializeField] GameObject playerHealth;

    private void Start()
    {
        isPaused = false;
        Time.timeScale = 1;
        Cursor.visible = isPaused;
        PlayerController.OnPlayerDestroyed += PlayerController_OnPlayerDestroyed;
        PlayerController.OnHealthChanged += PlayerController_OnHealthChanged;    
    }

    private void OnDestroy()
    {
        PlayerController.OnPlayerDestroyed -= PlayerController_OnPlayerDestroyed; 
        PlayerController.OnHealthChanged -= PlayerController_OnHealthChanged;
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

    public void QuitGame()
    {
        Application.Quit();
    }

    private void PlayerController_OnPlayerDestroyed()
    {
        Cursor.visible = true;
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    private void PlayerController_OnHealthChanged(int health)
    {
        //this could be done better
        if(health == 1)
        {
            playerHealth.gameObject.SetActive(false);
        }
        else
        {
            lastPlayerHealth.gameObject.SetActive(false);
        }
    }
}