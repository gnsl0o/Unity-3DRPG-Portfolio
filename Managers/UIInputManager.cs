using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class UIInputManager : MonoBehaviour
{
    #region MenuField

    public GameObject menuUIPrefab; // �г��� ������ �޴� UI�� ������
    private GameObject menuUIInstance; // �ν��Ͻ� ����
    private RectTransform menuPanelRect; // �޴� �г��� RectTransform (ĵ������ �ƴ� �г��� ����)
    private CanvasGroup menuCanvasGroup; // �޴� �г��� CanvasGroup
    private bool isMenuVisible = false; // �޴��� ���� ���̴��� ����

    public float slideDuration = 0.5f; // �����̵� �ִϸ��̼� �ð�
    public float fadeDuration = 0.5f; // ���̵� �ִϸ��̼� �ð�

    private Vector2 offScreenPosition; // �޴��� ȭ�� �ۿ� ���� ��ġ
    private Vector2 onScreenPosition; // �޴��� ȭ�� �ȿ� ���� ��ġ

    #endregion

    private void Start()
    {
        // �޴� ������ �ν��Ͻ�ȭ �� �ʱ�ȭ
        menuUIInstance = Instantiate(menuUIPrefab);

        // �޴� UI �� �г��� ����
        menuPanelRect = menuUIInstance.transform.Find("MenuPanel").GetComponent<RectTransform>();
        menuCanvasGroup = menuPanelRect.GetComponent<CanvasGroup>();

        // ��ġ ����
        onScreenPosition = menuPanelRect.anchoredPosition;
        offScreenPosition = new Vector2(-menuPanelRect.rect.width, menuPanelRect.anchoredPosition.y);

        // �ʱ� ���� (�޴��� ��Ȱ��ȭ�ϰ� ȭ�� ������ �̵���Ŵ)
        menuPanelRect.anchoredPosition = offScreenPosition;
        menuCanvasGroup.alpha = 0f;
        menuUIInstance.SetActive(false); // ó������ ��Ȱ��ȭ
    }

    public void MovePressed(InputAction.CallbackContext context)
    {
        if (context.performed || context.canceled)
        {
            GameEventsManager.instance.inputEvents.MovePressed(context.ReadValue<Vector2>());
        }
    }

    public void SubmitPressed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GameEventsManager.instance.inputEvents.SubmitPressed();
        }
    }

    public void QuestLogTogglePressed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GameEventsManager.instance.inputEvents.QuestLogTogglePressed();
        }
    }

    public void MenuTogglePressed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // �޴��� ������ ���� ���� ����, ���� ���� ����
            if (isMenuVisible)
            {
                HideMenu();
            }
            else
            {
                ShowMenu();
            }

            isMenuVisible = !isMenuVisible;
        }
    }

    private void ShowMenu()
    {
        // �޴� Ȱ��ȭ �� �ִϸ��̼� ����
        menuUIInstance.SetActive(true); // �޴��� Ȱ��ȭ

        // DOTween Sequence�� ����Ͽ� �����̵�� ���̵带 ���ÿ� ����
        Sequence sequence = DOTween.Sequence();
        sequence.Join(menuPanelRect.DOAnchorPos(onScreenPosition, slideDuration).SetEase(Ease.OutExpo))
                .Join(menuCanvasGroup.DOFade(1f, fadeDuration)); // �����̵�� ���̵� �� ���ÿ� ����
    }

    private void HideMenu()
    {
        // DOTween Sequence�� ����Ͽ� �����̵�� ���̵带 ���ÿ� ����
        Sequence sequence = DOTween.Sequence();
        sequence.Join(menuPanelRect.DOAnchorPos(offScreenPosition, slideDuration).SetEase(Ease.InExpo))
                .Join(menuCanvasGroup.DOFade(0f, fadeDuration)) // �����̵�� ���̵� �ƿ� ���ÿ� ����
                .OnComplete(() =>
                {
                    menuUIInstance.SetActive(false); // �ִϸ��̼� ���� �� �޴� ��Ȱ��ȭ
                });
    }
}