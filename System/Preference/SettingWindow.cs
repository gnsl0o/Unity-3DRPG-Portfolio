using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingWindow : MonoBehaviour
{
    private bool isOpened = false;
    public GameObject canvas;
    //public CameraController camCon;
    public void OpenSetting()
    {
        Time.timeScale = 0;
        canvas.SetActive(true);
        isOpened = true;
    }

    public void CloseSetting()
    {
        Time.timeScale = 1;
        canvas.SetActive(false);
        isOpened = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && isOpened == false)
        {
            OpenSetting();
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && isOpened == true)
        {
            CloseSetting();
        }
    }
}
