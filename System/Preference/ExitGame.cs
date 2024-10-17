using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitGame : MonoBehaviour
{
    public GameObject exitUIPanel;
    public CanvasGroup canvasGroup;
    public void ClikExit()
    {
        exitUIPanel.SetActive(true);
        canvasGroup.interactable = false;
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void ExitPanel()
    {
        exitUIPanel.SetActive(false);
        canvasGroup.interactable = true;
    }
}
