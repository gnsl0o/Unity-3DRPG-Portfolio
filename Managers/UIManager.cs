using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    public static UIManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindAnyObjectByType<UIManager>();
                if(instance == null)
                {
                    Debug.Log("UIManager�� ���� �������� ����");
                }
            }

            return instance;
        }
    }

    private Stack<UIPopup> openPopups = new Stack<UIPopup>();
    private Queue<UIPopup> pendingPopups = new Queue<UIPopup>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseLastOpenedPopup();
        }
    }

    public void OpenPopup(UIPopup popup)
    {
        if (popup != null)
        {
            Debug.Log(popup + " �������");
            popup.Open();
            openPopups.Push(popup);
        }
    }

    public void ClosePopup(UIPopup popup)
    {
        if (popup != null && openPopups.Contains(popup))
        {
            popup.Close();
            openPopups.Pop();

            if(pendingPopups.Count > 0)
            {
                OpenPopup(pendingPopups.Dequeue());
            }
        }
    }

    private void CloseLastOpenedPopup()
    {
        if(openPopups.Count > 0)
        {
            ClosePopup(openPopups.Peek());
        }
    }

    public void CloseAllOpenPopups()
    {
        while(openPopups.Count > 0)
        {
            ClosePopup(openPopups.Peek());
        }
    }

    public void ReservePopup(UIPopup popup)
    {
        if(popup != null)
        {
            pendingPopups.Enqueue(popup);
        }
    }
}
