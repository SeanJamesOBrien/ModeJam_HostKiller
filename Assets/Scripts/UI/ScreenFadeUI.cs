using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFadeUI : MonoBehaviour
{
    CanvasGroup canvas;
    Image image;
    [SerializeField] float fadeDelay = 0.25f;
    [SerializeField] float fadeDuration = 0.25f;

    private void Awake()
    {
        image = GetComponent<Image>();
        image.raycastTarget = false;
        canvas = GetComponent<CanvasGroup>();
        canvas.alpha = 0;
    }

    public IEnumerator LerpFade(bool isFadeIn)
    {
        float time = 0;
        float startValue = Convert.ToSingle(!isFadeIn);
        float endValue = Convert.ToSingle(isFadeIn);
        
        while (time < fadeDelay)
        {
            time += Time.unscaledDeltaTime;
            yield return null;
        }
        time = 0;

        while (time < fadeDuration)
        {
            canvas.alpha = Mathf.Lerp(startValue, endValue, time / fadeDuration);
            time += Time.unscaledDeltaTime;
            yield return null;
        }
        canvas.alpha = endValue;
    }
}