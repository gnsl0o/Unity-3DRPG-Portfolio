using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, IInteraction
{
    Animator anim;

    [SerializeField]
    private string name = "레이저";

    [SerializeField]
    private int _dialogueIndex = 1; // 인스펙터에서 할당 가능하도록 설정된 변수

    public int dialogueIndex // IInteraction의 dialogueIndex 구현
    {
        get { return _dialogueIndex; }
        set { _dialogueIndex = value; }
    }

    private bool isFirstTimeMeeting = true; // 처음 만나는지 여부

    // 상점에서 판매할 아이템들을 인스펙터에서 할당
    [SerializeField]
    private ItemSO[] itemsForSale; // 상점 아이템 배열

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Interact()
    {
        anim.SetTrigger("Interaction");

        if (isFirstTimeMeeting)
        {
            // 첫 대화 시 인스펙터에서 설정한 dialogueIndex 사용
            isFirstTimeMeeting = false; // 첫 대화 이후 상태로 전환
            InitializeShop(); // 상점 초기화
        }
        else
        {
            // 첫 만남이 아니면 반복 대화
            dialogueIndex = 2; // 반복 대화 인덱스
        }
    }

    private void InitializeShop()
    {
        // 상점 UI를 초기화하는 로직
        ShopManager.instance.InitializeShop(itemsForSale);
    }
}