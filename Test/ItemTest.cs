using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTest : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("������ �߰�");
            ItemManager.instance.AddItem(2);
        }
        if(Input.GetKeyDown(KeyCode.Y))
        {
            Debug.Log("������ ����");
            ItemManager.instance.AddItem(2);
        }
    }
}
