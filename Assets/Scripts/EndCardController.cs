using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCardController : MonoBehaviour
{
    void OnEndScreen()
    {
        SceneController.Instance.LoadNextScene(K.MainMenuScene);
    }
}
