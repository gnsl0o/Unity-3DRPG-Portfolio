using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPopup : MonoBehaviour
{
    [SerializeField] private GameObject popupCanvas;
    [SerializeField] private DOTweenAnimation doTweenOpenAnimation;
    [SerializeField] private DOTweenAnimation doTweenCloseAnimation;

    // �˾��� ���� �� ȣ��
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

    // �˾��� ���� �� ȣ��
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

    // �ִϸ��̼� �̺�Ʈ�� ȣ���� �޼���
    public void OnCloseAnimationFinished()
    {
        UIUtilities.SetUIActive(popupCanvas, false);
    }

    // �˾� ������ ������ ����� �� ȣ��� �޼���
    public void OnButtonClicked()
    {
        // ��ư Ŭ�� �� ������ ������ ���⿡ �߰�
    }
}