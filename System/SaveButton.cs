using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class SaveButton : MonoBehaviour
{
    public TextMeshProUGUI buttonText;
    private int currentSlot;
    public bool isLoadOperation = false;

    public void SetCurrentSlot(int slot)
    {
        currentSlot = slot;
    }

    public void SetIsLoadOperation(bool isLoad)
    {
        isLoadOperation = isLoad;
    }

    public void OnSaveButtonClicked()
    {
        if (isLoadOperation)
        {
            DataController.Instance.LoadGameData(currentSlot);
        }
        else
        {
            DataController.Instance.SaveGameData(currentSlot);
            UpdateButtonText();
        }
    }

    private void UpdateButtonText()
    {
        string currentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        buttonText.text = "Last Save: " + currentTime;
    }
}
