using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    public CanvasGroup image;
    public float fadeOutDuration = 1.0f;

    private void Start()
    {

    }

    private void OnEnable()
    {
        StartCoroutine(FadeOutRoutine());
    }
    IEnumerator FadeOutRoutine()
    {
        Debug.Log("»£√‚");
        float currentTime = 0f;

        while (currentTime < fadeOutDuration)
        {
            currentTime += Time.deltaTime;
            image.alpha = Mathf.Lerp(0f, 1f, currentTime / fadeOutDuration);
            yield return null;
        }
    }
}
