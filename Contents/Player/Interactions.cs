using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Burst.CompilerServices;

public class Interactions : MonoBehaviour
{
    public PlayerContext playerContext;
    public TextMeshProUGUI dialogueText;
    public Camera mainCamera;
    private BoxCollider boxCollider;

    private string talk = "��ȭ�ϱ�";

    private bool isDialogueActive = false; // �ؽ�Ʈ�� ī�޶� �ٶ󺸴� �Լ��� Ʈ����

    IInteraction interactObj; // ��ȣ�ۿ� ��ü�� ��� ����

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if (isDialogueActive) // ��ȭ ���°� Ȱ��ȭ�� ��쿡�� ó��
        {
            Vector3 directionToCamera = mainCamera.transform.position - dialogueText.transform.position;
            directionToCamera.y = 0;
            Quaternion rotationToCamera = Quaternion.LookRotation(directionToCamera);

            rotationToCamera = Quaternion.Euler(rotationToCamera.eulerAngles.x, rotationToCamera.eulerAngles.y + 180f, rotationToCamera.eulerAngles.z);
            dialogueText.transform.rotation = rotationToCamera;
        }

        InteractPlayer();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            if (playerContext.actionStateMachine.CurrentState == playerContext.actionStateMachine.SheathedState)
            {
                dialogueText.transform.position = other.transform.position + Vector3.up * 2;
                dialogueText.text = talk;
                interactObj = other.GetComponent<IInteraction>();
                UpdateDialogueState(true); // ��ȭ ���� Ȱ��ȭ
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            DisableInteraction();
        }
    }

    public void DisableInteraction()
    {
        dialogueText.text = "";
        interactObj = null;
        UpdateDialogueState(false);
    }

    public void DialougeEnd()
    {
        Debug.Log("��ȭ�� �����ϴ�");
        playerContext.actionStateMachine.SetState(playerContext.actionStateMachine.SheathedState); // ��ȭ�� ����Ǹ� �÷��̾ ������ ���·� �ǵ���
        UpdateDialogueState(false); // ��ȭ ���� ��Ȱ��ȭ
        interactObj = null;

        if (boxCollider != null)
        {
            boxCollider.enabled = false;
            boxCollider.enabled = true;
        }
    }

    // E Ű�� ������ ��ȣ�ۿ�
    private void InteractPlayer()
    {
        if (Input.GetKeyDown(KeyCode.E) && interactObj != null)
        {
            playerContext.actionStateMachine.SetState(playerContext.actionStateMachine.ConversationState); // �÷��̾��� ���¸� ������Ʈ
            interactObj.Interact();
            dialogueText.text = "";
            DialogueManager.Instance.DisplayDialogue(interactObj.name,1 ,0);
        }
    }

    // ��ȭ ���� ������Ʈ �޼���
    public void UpdateDialogueState(bool active)
    {
        if (isDialogueActive != active)
        {
            isDialogueActive = active;
        }
    }
}
