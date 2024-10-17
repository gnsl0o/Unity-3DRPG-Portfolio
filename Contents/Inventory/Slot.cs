using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Slot : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
{
    [SerializeField] 
    Image slotImage; // 슬롯에 들어갈 아이템의 이미지

    [SerializeField]
    public Image draggingIcon; // 드래그 중인 아이콘

    public static Slot draggedSlot; // 드래그 시작한 슬롯

    public int slotIndex; // 이 슬롯의 인덱스

    public UIPopup selectPanel;

    private int _item = -1; // 슬롯에서 해당 아이템의 ID를 참조하는 변수
    public int item
    {
        get { return _item; }
        set
        {
            _item = value;
        }
    }

    // 슬롯의 아이콘을 업데이트하는 메서드
    public void UpdateIcon(Sprite icon)
    {
        if (icon != null)
        {
            Debug.Log("여기까지 호출" + icon.ToString());
            slotImage.sprite = icon; // 스프라이트 할당
            slotImage.color = new Color(1, 1, 1, 1); // 아이콘이 있을 경우 이미지를 보이게 함
        }
        else
        {
            slotImage.sprite = null; // 또는 기본 빈 슬롯의 아이콘으로 설정
            slotImage.color = new Color(1, 1, 1, 0); // 아이콘이 없을 경우 투명도를 0%로 설정하여 불투명하게 만듦
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

            // 드롭 위치에 있는 슬롯을 찾음
            Slot dropSlot = eventData.pointerEnter?.GetComponent<Slot>();

            // 드롭 위치가 유효한 슬롯이고 다른 슬롯이라면 아이템 정보 교환
            if (dropSlot != null && dropSlot != draggedSlot)
            {
                Debug.Log("진입2");
                // 아이템 정보 교환 로직
                int tempItemID = dropSlot.item; // 임시로 dropSlot의 아이템 ID 저장
                Sprite tempIcon = dropSlot.slotImage.sprite; // 임시로 dropSlot의 아이콘 저장

                // dropSlot 업데이트
                dropSlot.item = draggedSlot.item;
                dropSlot.UpdateIcon(draggedSlot.slotImage.sprite);

                // draggedSlot 업데이트
                draggedSlot.item = tempItemID;
                draggedSlot.UpdateIcon(tempIcon);
            }
            else
            {
                // 드롭이 유효하지 않다면 원래 상태로 복원
                slotImage.color = new Color(1, 1, 1, 1); // 아이콘 색상 복원
            }

            draggedSlot = null;
        }

        // 드롭 위치에 따른 추가 로직 처리
        // 예를 들어, 드롭 위치의 슬롯 정보 확인 및 itemId 업데이트 등
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(item != -1)
        {

            // useButton의 위치를 클릭한 위치로 설정
            RectTransform useButtonRectTransform = selectPanel.gameObject.GetComponent<RectTransform>();
            if (useButtonRectTransform != null)
            {
                // 클릭한 위치를 화면 좌표에서 해당 캔버스 좌표계로 변환
                Vector2 localPoint;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    useButtonRectTransform.parent as RectTransform, // 부모 RectTransform
                    eventData.position, // 클릭한 스크린 좌표
                    eventData.pressEventCamera, // 이벤트 카메라
                    out localPoint); // 변환된 로컬 포인트

                // useButton의 로컬 위치 설정
                useButtonRectTransform.localPosition = new Vector2(localPoint.x, localPoint.y - 30);

                // useButton 활성화
                if (selectPanel.gameObject.activeSelf)
                {
                    UIManager.Instance.ClosePopup(selectPanel);
                }
                else
                {
                    UIManager.Instance.OpenPopup(selectPanel);
                }


                SlotManager.instance.SetCurrentSlot(this); // 현재 슬롯을 설정
            }
        }
    }
}
