using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUIController : MonoBehaviour
{
    [SerializeField] GameObject creditPanel;

    private void Start()
    {
        ProgressionController.Instance.Level = 0;
    }

    public void StartGame()
    {
        SceneController.Instance.LoadNextScene(K.TutorialScene);
        creditPanel.SetActive(false);
    }

    public void ToggleCredits()
    {
        creditPanel.SetActive(!creditPanel.activeSelf);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
