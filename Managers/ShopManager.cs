using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public static ShopManager instance;

    public GameObject shopUI;
    public Transform itemContainer;
    public GameObject itemPrefab;

    // ������ �� ���� ǥ�ø� ���� UI
    public UnityEngine.UI.Image itemImageL; // ū �̹���
    public TextMeshProUGUI itemPrice;
    public TextMeshProUGUI itemDescription;

    private void Awake()
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

    public void InitializeShop(ItemSO[] items)
    {
        foreach (Transform child in itemContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (ItemSO item in items)
        {
            GameObject itemGO = Instantiate(itemPrefab, itemContainer);

            // ������ UI �������� ������Ʈ ����
            UnityEngine.UI.Image itemImageS = itemGO.transform.Find("ItemImageS").GetComponent<UnityEngine.UI.Image>();
            TextMeshProUGUI itemName = itemGO.transform.Find("ItemName").GetComponent<TextMeshProUGUI>();

            // ������ ���� ����
            itemImageS.sprite = item.sprite; // ������ �̹��� ����
            itemName.text = item.name; // ������ �̸� ����

            UnityEngine.UI.Button itemButton = itemGO.GetComponent<UnityEngine.UI.Button>();

            itemButton.onClick.AddListener(() => ShowItemDetails(item));
        }
    }

    private void ShowItemDetails(ItemSO item)
    {
        itemImageL.sprite = item.sprite; // ū �̹��� ����
        itemPrice.text = item.purchasePrice.ToString(); // ���� ����
        itemDescription.text = item.description; // ���� ����
    }

    public void ShowShop()
    {
        shopUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void HideShop()
    {
        shopUI.SetActive(false);
        Time.timeScale = 1f;
    }
}
