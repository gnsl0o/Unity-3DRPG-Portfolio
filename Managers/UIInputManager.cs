using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class UIInputManager : MonoBehaviour
{
    #region MenuField

    public GameObject menuUIPrefab; // 패널을 포함한 메뉴 UI의 프리팹
    private GameObject menuUIInstance; // 인스턴스 참조
    private RectTransform menuPanelRect; // 메뉴 패널의 RectTransform (캔버스가 아닌 패널을 참조)
    private CanvasGroup menuCanvasGroup; // 메뉴 패널의 CanvasGroup
    private bool isMenuVisible = false; // 메뉴가 현재 보이는지 여부

    public float slideDuration = 0.5f; // 슬라이드 애니메이션 시간
    public float fadeDuration = 0.5f; // 페이드 애니메이션 시간

    private Vector2 offScreenPosition; // 메뉴가 화면 밖에 있을 위치
    private Vector2 onScreenPosition; // 메뉴가 화면 안에 있을 위치

    #endregion

    private void Start()
    {
        // 메뉴 프리팹 인스턴스화 및 초기화
        menuUIInstance = Instantiate(menuUIPrefab);

        // 메뉴 UI 내 패널을 참조
        menuPanelRect = menuUIInstance.transform.Find("MenuPanel").GetComponent<RectTransform>();
        menuCanvasGroup = menuPanelRect.GetComponent<CanvasGroup>();

        // 위치 설정
        onScreenPosition = menuPanelRect.anchoredPosition;
        offScreenPosition = new Vector2(-menuPanelRect.rect.width, menuPanelRect.anchoredPosition.y);

        // 초기 설정 (메뉴를 비활성화하고 화면 밖으로 이동시킴)
        menuPanelRect.anchoredPosition = offScreenPosition;
        menuCanvasGroup.alpha = 0f;
        menuUIInstance.SetActive(false); // 처음에는 비활성화
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
            // 메뉴가 보이지 않을 때는 열고, 보일 때는 닫음
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
        // 메뉴 활성화 및 애니메이션 실행
        menuUIInstance.SetActive(true); // 메뉴를 활성화

        // DOTween Sequence를 사용하여 슬라이드와 페이드를 동시에 실행
        Sequence sequence = DOTween.Sequence();
        sequence.Join(menuPanelRect.DOAnchorPos(onScreenPosition, slideDuration).SetEase(Ease.OutExpo))
                .Join(menuCanvasGroup.DOFade(1f, fadeDuration)); // 슬라이드와 페이드 인 동시에 실행
    }

    private void HideMenu()
    {
        // DOTween Sequence를 사용하여 슬라이드와 페이드를 동시에 실행
        Sequence sequence = DOTween.Sequence();
        sequence.Join(menuPanelRect.DOAnchorPos(offScreenPosition, slideDuration).SetEase(Ease.InExpo))
                .Join(menuCanvasGroup.DOFade(0f, fadeDuration)) // 슬라이드와 페이드 아웃 동시에 실행
                .OnComplete(() =>
                {
                    menuUIInstance.SetActive(false); // 애니메이션 종료 후 메뉴 비활성화
                });
    }
}