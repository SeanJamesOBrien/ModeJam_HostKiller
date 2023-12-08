using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static event Action OnSceneChange = delegate { };

    [SerializeField] float transitionBuffer;
    [SerializeField] ScreenFadeUI ui;

    public static SceneController Instance { get; private set; }
    private void Awake()
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

    public void LoadNextScene(string sceneName)
    {
        OnSceneChange?.Invoke();
        StartCoroutine(StartLoad(sceneName));
    }

    IEnumerator StartLoad(string sceneName)
    {
        //Debug.Log(sceneName);
        yield return StartCoroutine(ui.LerpFade(true));

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            yield return new WaitForEndOfFrame();
        }
        float time = 0;
        while (time < transitionBuffer)
        {
            time += Time.unscaledDeltaTime;
            yield return null;
        }
        yield return StartCoroutine(ui.LerpFade(false));
    }
}