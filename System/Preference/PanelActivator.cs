using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems; // EventSystem ����� ����
using UnityEngine.UI; // UI ������Ʈ ����� ����

public class PanelActivator : MonoBehaviour,IPointerEnterHandler
{
    public GameObject activePanel; // Ȱ��ȭ�� �г�
    public TextMeshProUGUI originText; // Ȱ��ȭ�� �г��� �ؽ�Ʈ ������Ʈ
    public TextMeshProUGUI newText; // ���콺 ������ ǥ�õ� �ؽ�Ʈ

    private Vector3 originalPosition; // Ȱ��ȭ �г��� ���� ��ġ ����

    void Start()
    {
        originalPosition = activePanel.transform.position;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        activePanel.SetActive(true); // �г� Ȱ��ȭ
        activePanel.transform.position = transform.position + new Vector3(0, 2f, 0); ; // ���콺 ������ ��ġ�� �̵�
        newText.text = originText.text; // �ؽ�Ʈ ����
    }
}
