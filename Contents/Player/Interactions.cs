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

    private string talk = "대화하기";

    private bool isDialogueActive = false; // 텍스트가 카메라를 바라보는 함수의 트리거

    IInteraction interactObj; // 상호작용 객체를 담는 변수

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        if (isDialogueActive) // 대화 상태가 활성화된 경우에만 처리
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
                UpdateDialogueState(true); // 대화 상태 활성화
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
        Debug.Log("대화를 끝냅니다");
        playerContext.actionStateMachine.SetState(playerContext.actionStateMachine.SheathedState); // 대화가 종료되면 플레이어를 원래의 상태로 되돌림
        UpdateDialogueState(false); // 대화 상태 비활성화
        interactObj = null;

        if (boxCollider != null)
        {
            boxCollider.enabled = false;
            boxCollider.enabled = true;
        }
    }

    // E 키를 누르면 상호작용
    private void InteractPlayer()
    {
        if (Input.GetKeyDown(KeyCode.E) && interactObj != null)
        {
            playerContext.actionStateMachine.SetState(playerContext.actionStateMachine.ConversationState); // 플레이어의 상태를 업데이트
            interactObj.Interact();
            dialogueText.text = "";
            DialogueManager.Instance.DisplayDialogue(interactObj.name,1 ,0);
        }
    }

    // 대화 상태 업데이트 메서드
    public void UpdateDialogueState(bool active)
    {
        if (isDialogueActive != active)
        {
            isDialogueActive = active;
        }
    }
}
