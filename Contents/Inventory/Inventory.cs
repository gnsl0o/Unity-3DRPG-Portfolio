using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<ItemSO> inventory = new List<ItemSO>();
    public Dictionary<ItemSO, int> itemCount = new Dictionary<ItemSO, int>();

    [SerializeField]
    private Transform slotParent;
    [SerializeField]
    private Slot[] slots;

#if UNITY_EDITOR
    private void OnValidate()
    {
        slots = slotParent.GetComponentsInChildren<Slot>();
    }
#endif

    public void AddItem(ItemSO item, int quantity)
    {
        if (item != null)
        {
            // �������� �̹� �κ��丮�� �����ϴ��� Ȯ��
            if (!itemCount.ContainsKey(item))
            {
                // �κ��丮 �뷮 Ȯ�� �� ������ �߰�
                if (inventory.Count < 20)
                {
                    inventory.Add(item);
                    itemCount[item] = quantity;
                    UpdateUI(item);
                }
                else
                {
                    Debug.Log("�κ��丮�� ���� á���ϴ�.");
                    return;
                }
            }
            else
            {
                // �������� �̹� �����ϴ� ���, ������ ������Ʈ
                itemCount[item] += quantity;
            }
        }
        else
        {
            Debug.Log("�������� ã�� �� �����ϴ�.");
        }
    }

    public void RemoveItem(Slot slot)
    {
        ItemSO item = ItemManager.instance.GetItem(slot.item);

        if (itemCount[item] > 1)
        {
            itemCount[item] -= 1;
        }
        else
        {
            slot.ClearIcon();
            inventory.Remove(item);
            itemCount.Remove(item);
        }
    }

    // �κ��丮 UI ������Ʈ
    public void UpdateUI(ItemSO item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == -1 || slots[i].item == item.itemId)
            {
                slots[i].item = item.itemId;
                slots[i].UpdateIcon(item.sprite);

                // �����۰� ������ ���� ���� ������Ʈ
                item.slotIndex = i;
                Debug.Log(item.slotIndex + " ���� ����");
                return;
            }
        }
    }
}
