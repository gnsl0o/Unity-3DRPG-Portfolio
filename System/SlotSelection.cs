using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotSelection : MonoBehaviour
{
    public Button[] slotButtons;

    private void Start()
    {
        for(int i = 0; i < slotButtons.Length; i++)
        {
            int slotIndex = i;
            slotButtons[i].onClick.AddListener(() => OnSlotSelected(slotIndex));
        }
    }

    private void OnSlotSelected(int slot)
    {

    }
}
