using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    private void OnDestroy()
    {
        Cursor.visible = true;
        SceneController.Instance.LoadNextScene(K.MainMenuScene);
    }
}
