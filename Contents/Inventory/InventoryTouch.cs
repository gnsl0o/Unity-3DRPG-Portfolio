using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryTouch : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public GameObject itemDecriptionPanel; // 아이템 설명 패널
    public GameObject interactionUI; // 상호작용 UI
    public UIPopup selectPanel;

    // 마우스 오버 시
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemDecriptionPanel != null)
        {
            itemDecriptionPanel.SetActive(true);
        }
    }

    // 마우스 아웃 시
    public void OnPointerExit(PointerEventData eventData)
    {
        if (itemDecriptionPanel != null)
        {
            itemDecriptionPanel.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 오른쪽 마우스 버튼 클릭을 확인
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("작동");
            // 마우스 클릭 위치를 화면 좌표로 변환
            Vector2 mousePosition = eventData.position;

            // UI의 RectTransform을 가져옴
            RectTransform rectTransform = interactionUI.GetComponent<RectTransform>();

            // Canvas의 RectTransform을 가져옴
            RectTransform canvasRectTransform = interactionUI.transform.parent.GetComponent<RectTransform>();

            // 스크린 좌표를 Canvas의 좌표계로 변환
            Vector2 canvasPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, mousePosition, null, out canvasPosition);

            // UI 요소의 위치를 마우스 클릭 위치로 설정
            rectTransform.localPosition = new Vector2(canvasPosition.x, canvasPosition.y - 25);

            // UI 활성화/비활성화 토글
            UIManager.Instance.OpenPopup(selectPanel);
        }
    }
}
