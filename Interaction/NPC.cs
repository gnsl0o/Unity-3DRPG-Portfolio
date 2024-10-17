using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, IInteraction
{
    Animator anim;

    [SerializeField]
    private string name = "������";

    [SerializeField]
    private int _dialogueIndex = 1; // �ν����Ϳ��� �Ҵ� �����ϵ��� ������ ����

    public int dialogueIndex // IInteraction�� dialogueIndex ����
    {
        get { return _dialogueIndex; }
        set { _dialogueIndex = value; }
    }

    private bool isFirstTimeMeeting = true; // ó�� �������� ����

    // �������� �Ǹ��� �����۵��� �ν����Ϳ��� �Ҵ�
    [SerializeField]
    private ItemSO[] itemsForSale; // ���� ������ �迭

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Interact()
    {
        anim.SetTrigger("Interaction");

        if (isFirstTimeMeeting)
        {
            // ù ��ȭ �� �ν����Ϳ��� ������ dialogueIndex ���
            isFirstTimeMeeting = false; // ù ��ȭ ���� ���·� ��ȯ
            InitializeShop(); // ���� �ʱ�ȭ
        }
        else
        {
            // ù ������ �ƴϸ� �ݺ� ��ȭ
            dialogueIndex = 2; // �ݺ� ��ȭ �ε���
        }
    }

    private void InitializeShop()
    {
        // ���� UI�� �ʱ�ȭ�ϴ� ����
        ShopManager.instance.InitializeShop(itemsForSale);
    }
}