using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTest : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("아이템 추가");
            ItemManager.instance.AddItem(2);
        }
        if(Input.GetKeyDown(KeyCode.Y))
        {
            Debug.Log("아이템 제거");
            ItemManager.instance.AddItem(2);
        }
    }
}
