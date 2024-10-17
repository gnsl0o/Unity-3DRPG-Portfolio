using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryTouch : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public GameObject itemDecriptionPanel; // ������ ���� �г�
    public GameObject interactionUI; // ��ȣ�ۿ� UI
    public UIPopup selectPanel;

    // ���콺 ���� ��
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemDecriptionPanel != null)
        {
            itemDecriptionPanel.SetActive(true);
        }
    }

    // ���콺 �ƿ� ��
    public void OnPointerExit(PointerEventData eventData)
    {
        if (itemDecriptionPanel != null)
        {
            itemDecriptionPanel.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // ������ ���콺 ��ư Ŭ���� Ȯ��
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("�۵�");
            // ���콺 Ŭ�� ��ġ�� ȭ�� ��ǥ�� ��ȯ
            Vector2 mousePosition = eventData.position;

            // UI�� RectTransform�� ������
            RectTransform rectTransform = interactionUI.GetComponent<RectTransform>();

            // Canvas�� RectTransform�� ������
            RectTransform canvasRectTransform = interactionUI.transform.parent.GetComponent<RectTransform>();

            // ��ũ�� ��ǥ�� Canvas�� ��ǥ��� ��ȯ
            Vector2 canvasPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, mousePosition, null, out canvasPosition);

            // UI ����� ��ġ�� ���콺 Ŭ�� ��ġ�� ����
            rectTransform.localPosition = new Vector2(canvasPosition.x, canvasPosition.y - 25);

            // UI Ȱ��ȭ/��Ȱ��ȭ ���
            UIManager.Instance.OpenPopup(selectPanel);
        }
    }
}
