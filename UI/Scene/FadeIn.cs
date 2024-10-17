using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    public CanvasGroup image;
    public float fadeInDuration = 1.0f;

    private void Start()
    {
        Time.timeScale = 0;
        StartCoroutine(FadeInRoutine());
        Time.timeScale = 1;
    }

    IEnumerator FadeInRoutine()
    {
        float currentTime = 0f;

        while(currentTime < fadeInDuration)
        {
            currentTime += Time.deltaTime;
            image.alpha = Mathf.Lerp(0.8f,0,currentTime/fadeInDuration);
            yield return null;
        }
    }
}
