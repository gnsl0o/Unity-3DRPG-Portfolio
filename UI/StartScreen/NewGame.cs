using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGame : MonoBehaviour
{
    public AudioClip bgm;

    private void Start()
    {

    }
    public void StartNewGame()
    {
        ScreenManager.instance._LoadScreenTexture();
        LoadingScreen.LoadScene("NewGame");
    }
}
