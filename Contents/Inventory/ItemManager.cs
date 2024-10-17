using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemEntry
{
    public ItemSO item; // 아이템 ScriptableObject
}

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    // 인스펙터에서 수정 가능한 리스트
    public List<ItemEntry> itemEntries = new List<ItemEntry>();
    // 실제 게임에서 사용할 Dictionary
    private Dictionary<int, ItemSO> itemList = new Dictionary<int, ItemSO>();

    public Inventory consumableInventory;
    public Inventory equipmentInventory;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // 리스트를 Dictionary로 변환
            foreach (ItemEntry entry in itemEntries)
            {
                if (!itemList.ContainsKey(entry.item.itemId)) // 중복된 키 방지
                {
                    itemList.Add(entry.item.itemId, entry.item);
                }
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public ItemSO GetItem(int itemId)
    {
        if (itemList.TryGetValue(itemId, out ItemSO item))
        {
            return item;
        }

        return null;
    }
    public void AddItem(int itemId)
    {
        ItemSO item = ItemManager.instance.GetItem(itemId);

        switch (item.itemType)
        {
            case ItemSO.ItemType.Equipment:
                equipmentInventory.AddItem(item,1);
                break;
            case ItemSO.ItemType.Consumable:
                consumableInventory.AddItem(item,1);
                break;
            // 다른 ItemType 처리...
            default:
                Debug.LogWarning("Unhandled item type!");
                break;
        }
    }
}