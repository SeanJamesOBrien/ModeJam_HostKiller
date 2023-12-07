using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUIController : MonoBehaviour
{
    [SerializeField] GameObject creditPanel;
    public void StartGame()
    {
        SceneController.Instance.LoadNextScene(K.GameScene);
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
