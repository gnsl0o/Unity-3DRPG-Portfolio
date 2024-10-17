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
            // 아이템이 이미 인벤토리에 존재하는지 확인
            if (!itemCount.ContainsKey(item))
            {
                // 인벤토리 용량 확인 및 아이템 추가
                if (inventory.Count < 20)
                {
                    inventory.Add(item);
                    itemCount[item] = quantity;
                    UpdateUI(item);
                }
                else
                {
                    Debug.Log("인벤토리가 가득 찼습니다.");
                    return;
                }
            }
            else
            {
                // 아이템이 이미 존재하는 경우, 개수만 업데이트
                itemCount[item] += quantity;
            }
        }
        else
        {
            Debug.Log("아이템을 찾을 수 없습니다.");
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

    // 인벤토리 UI 업데이트
    public void UpdateUI(ItemSO item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == -1 || slots[i].item == item.itemId)
            {
                slots[i].item = item.itemId;
                slots[i].UpdateIcon(item.sprite);

                // 아이템과 슬롯의 매핑 정보 업데이트
                item.slotIndex = i;
                Debug.Log(item.slotIndex + " 번에 저장");
                return;
            }
        }
    }
}
