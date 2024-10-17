using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Slot : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
{
    [SerializeField] 
    Image slotImage; // ���Կ� �� �������� �̹���

    [SerializeField]
    public Image draggingIcon; // �巡�� ���� ������

    public static Slot draggedSlot; // �巡�� ������ ����

    public int slotIndex; // �� ������ �ε���

    public UIPopup selectPanel;

    private int _item = -1; // ���Կ��� �ش� �������� ID�� �����ϴ� ����
    public int item
    {
        get { return _item; }
        set
        {
            _item = value;
        }
    }

    // ������ �������� ������Ʈ�ϴ� �޼���
    public void UpdateIcon(Sprite icon)
    {
        if (icon != null)
        {
            Debug.Log("������� ȣ��" + icon.ToString());
            slotImage.sprite = icon; // ��������Ʈ �Ҵ�
            slotImage.color = new Color(1, 1, 1, 1); // �������� ���� ��� �̹����� ���̰� ��
        }
        else
        {
            slotImage.sprite = null; // �Ǵ� �⺻ �� ������ ���������� ����
            slotImage.color = new Color(1, 1, 1, 0); // �������� ���� ��� ������ 0%�� �����Ͽ� �������ϰ� ����
        }
    }

    public void ClearIcon()
    {
        slotImage.color = new Color(1, 1, 1, 0);
        item = -1;
        slotImage.sprite = null;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(item != -1 && draggingIcon != null)
        {
            draggedSlot = this;
            draggingIcon.sprite = slotImage.sprite;
            draggingIcon.gameObject.SetActive(true);
            slotImage.color = new Color(1, 1, 1, 0.5f);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        draggingIcon.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(draggingIcon != null && item != -1)
        {
            draggingIcon.gameObject.SetActive(false);

            // ��� ��ġ�� �ִ� ������ ã��
            Slot dropSlot = eventData.pointerEnter?.GetComponent<Slot>();

            // ��� ��ġ�� ��ȿ�� �����̰� �ٸ� �����̶�� ������ ���� ��ȯ
            if (dropSlot != null && dropSlot != draggedSlot)
            {
                Debug.Log("����2");
                // ������ ���� ��ȯ ����
                int tempItemID = dropSlot.item; // �ӽ÷� dropSlot�� ������ ID ����
                Sprite tempIcon = dropSlot.slotImage.sprite; // �ӽ÷� dropSlot�� ������ ����

                // dropSlot ������Ʈ
                dropSlot.item = draggedSlot.item;
                dropSlot.UpdateIcon(draggedSlot.slotImage.sprite);

                // draggedSlot ������Ʈ
                draggedSlot.item = tempItemID;
                draggedSlot.UpdateIcon(tempIcon);
            }
            else
            {
                // ����� ��ȿ���� �ʴٸ� ���� ���·� ����
                slotImage.color = new Color(1, 1, 1, 1); // ������ ���� ����
            }

            draggedSlot = null;
        }

        // ��� ��ġ�� ���� �߰� ���� ó��
        // ���� ���, ��� ��ġ�� ���� ���� Ȯ�� �� itemId ������Ʈ ��
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(item != -1)
        {

            // useButton�� ��ġ�� Ŭ���� ��ġ�� ����
            RectTransform useButtonRectTransform = selectPanel.gameObject.GetComponent<RectTransform>();
            if (useButtonRectTransform != null)
            {
                // Ŭ���� ��ġ�� ȭ�� ��ǥ���� �ش� ĵ���� ��ǥ��� ��ȯ
                Vector2 localPoint;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    useButtonRectTransform.parent as RectTransform, // �θ� RectTransform
                    eventData.position, // Ŭ���� ��ũ�� ��ǥ
                    eventData.pressEventCamera, // �̺�Ʈ ī�޶�
                    out localPoint); // ��ȯ�� ���� ����Ʈ

                // useButton�� ���� ��ġ ����
                useButtonRectTransform.localPosition = new Vector2(localPoint.x, localPoint.y - 30);

                // useButton Ȱ��ȭ
                if (selectPanel.gameObject.activeSelf)
                {
                    UIManager.Instance.ClosePopup(selectPanel);
                }
                else
                {
                    UIManager.Instance.OpenPopup(selectPanel);
                }


                SlotManager.instance.SetCurrentSlot(this); // ���� ������ ����
            }
        }
    }
}
