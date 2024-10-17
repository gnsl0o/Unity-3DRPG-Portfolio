using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public Slider progressBar;
    public Image loadingIcon;
    private bool loadComplete = false;
    private AsyncOperation asyncLoad; // 비동기 로드 작업을 참조하기 위한 변수

    public CanvasGroup image;
    public float fadeInDuration = 1.0f;
    private bool textOnBoard = false;

    private static string sceneToLoad;

    public static void LoadScene(string sceneName)
    {
        sceneToLoad = sceneName;
        SceneManager.LoadScene("Loading");
    }

    private void Start()
    {
        StartCoroutine(LoadAsyncScene(sceneToLoad));
    }

    private void Update()
    {
        if(loadComplete && !textOnBoard)
        {
            StartCoroutine(FadeInRoutine());
            textOnBoard = true;
        }

        if(loadComplete && Input.anyKeyDown)
        {
            // 사용자 입력을 받으면 씬 활성화를 허용
            asyncLoad.allowSceneActivation = true;
        }
    }

    IEnumerator LoadAsyncScene(string sceneName)
    {
        asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            progressBar.value = progress;
            loadingIcon.fillAmount = progress;

            if(asyncLoad.progress >= 0.9f)
            {
                loadComplete = true;
            }

            yield return null;
        }

        // 씬 로드 후 데이터를 적용
        DataController.Instance.ApplyLoadedData();
    }

    IEnumerator FadeInRoutine()
    {
        float currentTime = 0f;

        while (currentTime < fadeInDuration)
        {
            currentTime += Time.deltaTime;
            image.alpha = Mathf.Lerp(0f, 1f, currentTime / fadeInDuration);
            yield return null;
        }
    }
}
