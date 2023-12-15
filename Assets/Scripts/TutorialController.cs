using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public void StartGame()
    {
        ProgressionController.Instance.StartNextLevel();
    }
}
