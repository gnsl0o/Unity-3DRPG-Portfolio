using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopup : MonoBehaviour
{
    [SerializeField] private GameObject popupCanvas;
    [SerializeField] private DOTweenAnimation doTweenOpenAnimation;
    [SerializeField] private DOTweenAnimation doTweenCloseAnimation;

    // 팝업이 열릴 때 호출
    public void Open()
    {
        if (popupCanvas != null)
        {
            UIUtilities.SetUIActive(popupCanvas, true);
            if (doTweenOpenAnimation != null)
            {
                doTweenOpenAnimation.DORestart();
            }
        }
    }

    // 팝업이 닫힐 때 호출
    public void Close()
    {
        if (popupCanvas != null)
        {
            if (doTweenCloseAnimation != null)
            {
                doTweenCloseAnimation.DORestart();
                doTweenCloseAnimation.onComplete.AddListener(() =>
                {
                    UIUtilities.SetUIActive(popupCanvas, false);
                });
            }
            else
            {
                UIUtilities.SetUIActive(popupCanvas, false);
            }
        }
    }

    // 애니메이션 이벤트로 호출할 메서드
    public void OnCloseAnimationFinished()
    {
        UIUtilities.SetUIActive(popupCanvas, false);
    }

    // 팝업 내에서 동작이 수행될 때 호출될 메서드
    public void OnButtonClicked()
    {
        // 버튼 클릭 시 수행할 동작을 여기에 추가
    }
}