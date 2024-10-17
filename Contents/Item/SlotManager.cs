using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotManager : MonoBehaviour
{
    public static SlotManager instance;

    public ItemUsageHandler itemUsageHandler;
    public Inventory inventory;
    public GameObject Panel; // 버튼 패널

    public Slot currentSelectedSlot;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetCurrentSlot(Slot slot)
    {
        currentSelectedSlot = slot;
    }

    public void ActivateCurrentItem()
    {
        if (currentSelectedSlot != null && currentSelectedSlot.item != -1)
        {
            ItemSO item = ItemManager.instance.GetItem(currentSelectedSlot.item);
            itemUsageHandler.UseItem(item);
            inventory.RemoveItem(currentSelectedSlot);
            Panel.SetActive(false);
        }
    }
}