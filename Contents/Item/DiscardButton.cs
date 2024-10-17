using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscardButton : MonoBehaviour
{
    private Slot currentSelectedSlot;
    public SlotManager slotManager;

    public void DiscardItem()
    {
        currentSelectedSlot = slotManager.currentSelectedSlot;

        slotManager.inventory.RemoveItem(currentSelectedSlot);
        slotManager.Panel.SetActive(false);
    }
}
