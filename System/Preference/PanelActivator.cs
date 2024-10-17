using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems; // EventSystem 사용을 위함
using UnityEngine.UI; // UI 컴포넌트 사용을 위함

public class PanelActivator : MonoBehaviour,IPointerEnterHandler
{
    public GameObject activePanel; // 활성화될 패널
    public TextMeshProUGUI originText; // 활성화될 패널의 텍스트 컴포넌트
    public TextMeshProUGUI newText; // 마우스 오버시 표시될 텍스트

    private Vector3 originalPosition; // 활성화 패널의 원래 위치 저장

    void Start()
    {
        originalPosition = activePanel.transform.position;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        activePanel.SetActive(true); // 패널 활성화
        activePanel.transform.position = transform.position + new Vector3(0, 2f, 0); ; // 마우스 오버된 위치로 이동
        newText.text = originText.text; // 텍스트 변경
    }
}
