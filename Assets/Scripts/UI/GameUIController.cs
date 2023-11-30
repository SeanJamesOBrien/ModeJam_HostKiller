using System;
using UnityEditor;
using UnityEngine;

public class GameUIController : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;
    bool isPaused = false;

    private void Start()
    {
        isPaused = false;
        Time.timeScale = 1;
        Cursor.visible = isPaused;
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
}
