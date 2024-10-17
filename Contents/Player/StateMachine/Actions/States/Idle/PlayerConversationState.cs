using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerConversationState : PlayerAction
{
    public PlayerConversationState(PlayerActionStateMachine playerActionStateMachine)
    {
        stateMachine = playerActionStateMachine;
    }

    public override void EnterState()
    {
        Debug.Log("��ȭ ���¿� ����");
        stateMachine.player.Input.SwitchActionMap("Conversation");
        stateMachine.player.Input.ConversationActions.NextDialogue.performed += OnInteractionPerformed;
        stateMachine.player.Input.DisablePlayerActions();
    }

    public override void ExitState()
    {
        stateMachine.player.Input.ConversationActions.NextDialogue.performed -= OnInteractionPerformed;
    }

    private void OnInteractionPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("�ش� ���");
        DialogueManager.Instance.NextDialogue();
    }
}
