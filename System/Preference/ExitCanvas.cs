using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitCanvas : MonoBehaviour
{
    public GameObject exitUIPanel;
    public GameObject reOpenCanvas;
    public void ExitPanel()
    {
        exitUIPanel.SetActive(false);
        reOpenCanvas.SetActive(true);
    }
}
