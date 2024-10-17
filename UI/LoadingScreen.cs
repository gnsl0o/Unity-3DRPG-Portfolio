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
    private AsyncOperation asyncLoad; // �񵿱� �ε� �۾��� �����ϱ� ���� ����

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
            // ����� �Է��� ������ �� Ȱ��ȭ�� ���
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

        // �� �ε� �� �����͸� ����
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
