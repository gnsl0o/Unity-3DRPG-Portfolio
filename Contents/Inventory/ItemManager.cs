using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemEntry
{
    public ItemSO item; // ������ ScriptableObject
}

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    // �ν����Ϳ��� ���� ������ ����Ʈ
    public List<ItemEntry> itemEntries = new List<ItemEntry>();
    // ���� ���ӿ��� ����� Dictionary
    private Dictionary<int, ItemSO> itemList = new Dictionary<int, ItemSO>();

    public Inventory consumableInventory;
    public Inventory equipmentInventory;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // ����Ʈ�� Dictionary�� ��ȯ
            foreach (ItemEntry entry in itemEntries)
            {
                if (!itemList.ContainsKey(entry.item.itemId)) // �ߺ��� Ű ����
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
            // �ٸ� ItemType ó��...
            default:
                Debug.LogWarning("Unhandled item type!");
                break;
        }
    }
}