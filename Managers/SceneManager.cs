using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenManager : MonoBehaviour
{
    private static ScreenManager _instance;
    public static ScreenManager instance
    {
        get { return _instance; }
        set { _instance = value; }
    }

    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        AudioManager.instance.PlayBGM(SceneManager.GetActiveScene().name);    
    }

    public Texture2D ScreenTexture;

    IEnumerator CaptureScreen()
    {
        // �ؽ�ó �ڷ��� ������ ����
        Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        yield return new WaitForEndOfFrame();

        // ȭ���� �ȼ� �����͸� �о �ؽ�óȭ �ϴ� ����
        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
        texture.Apply();
        ScreenTexture = texture;
    }

    public void _LoadScreenTexture()
    {
        StartCoroutine(CaptureScreen());
    }
}
