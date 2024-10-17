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

    // 아이템 상세 정보 표시를 위한 UI
    public UnityEngine.UI.Image itemImageL; // 큰 이미지
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

            // 아이템 UI 프리팹의 컴포넌트 접근
            UnityEngine.UI.Image itemImageS = itemGO.transform.Find("ItemImageS").GetComponent<UnityEngine.UI.Image>();
            TextMeshProUGUI itemName = itemGO.transform.Find("ItemName").GetComponent<TextMeshProUGUI>();

            // 아이템 정보 설정
            itemImageS.sprite = item.sprite; // 아이템 이미지 설정
            itemName.text = item.name; // 아이템 이름 설정

            UnityEngine.UI.Button itemButton = itemGO.GetComponent<UnityEngine.UI.Button>();

            itemButton.onClick.AddListener(() => ShowItemDetails(item));
        }
    }

    private void ShowItemDetails(ItemSO item)
    {
        itemImageL.sprite = item.sprite; // 큰 이미지 설정
        itemPrice.text = item.purchasePrice.ToString(); // 가격 설정
        itemDescription.text = item.description; // 설명 설정
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
