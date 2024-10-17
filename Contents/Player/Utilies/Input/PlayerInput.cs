using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public PlayerInputActions InputActions { get; private set; }
    public PlayerInputActions.PlayerActions PlayerActions { get; private set; }
    public PlayerInputActions.BattleActions BattleActions { get; private set; }
    public PlayerInputActions.ConversationActions ConversationActions { get; private set; }
    public PlayerInputActions.AttackActions AttackActions { get; private set; }
    
    public event Action<string> OnInputReceived;

    private bool isInputEnabled = false;

    private void Awake()
    {
        InputActions = new PlayerInputActions();
        
        PlayerActions = InputActions.Player;
        BattleActions = InputActions.Battle;
        ConversationActions = InputActions.Conversation;
        AttackActions = InputActions.Attack;

        AttackActions.Attack.performed += context => OnInputReceived?.Invoke("Attack");
        AttackActions.Parry.performed += context => OnInputReceived?.Invoke("Parry");
    }

    private void OnEnable()
    {
        // InputActions.Enable();
        PlayerActions.Enable();
        isInputEnabled = true;
    }

    private void OnDisable()
    {
        InputActions.Disable();
        isInputEnabled = false;
    }

    private void OnDestroy()
    {
        foreach (var action in InputActions)
        {
            action.performed -= context => OnInputReceived?.Invoke(action.name);
        }
    }

    public void DisableActionFor(InputAction action, float seconds)
    {
        StartCoroutine(DisableAction(action, seconds));
    }

    private IEnumerator DisableAction(InputAction action, float seconds)
    {
        action.Disable();

        yield return new WaitForSeconds(seconds);

        action.Enable();
    }

    public void SwitchActionMap(string state)
    {
        switch (state)
        {
            case "Player":
                DisableExceptPlayerActions();
                PlayerActions.Enable();
                break;
            case "Battle":
                DisableExceptPlayerActions();
                BattleActions.Enable();
                break;
            case "Attack":
                DisableExceptPlayerActions();
                AttackActions.Enable();
                break;
            case "Conversation":
                DisableAllPlayerActions();
                ConversationActions.Enable();
                break;
        }
    }

    public void DisableExceptPlayerActions()
    {
        BattleActions.Disable();
        AttackActions.Disable();
        ConversationActions.Disable();
    }

    public void DisableAllPlayerActions()
    {
        PlayerActions.Disable();
        BattleActions.Disable();
        AttackActions.Disable();
        ConversationActions.Disable();
    }

    public void EnableAllPlayerActions()
    {
        PlayerActions.Enable();
        BattleActions.Enable();
    }

    public void EnableDefenseState()
    {
        PlayerActions.Disable();
        AttackActions.Enable();
    }

    public void DisablePlayerActions()
    {
        Debug.Log("DisablePlayerActions »£√‚");
        PlayerActions.Disable();
    }

    public void EnablePlayerActions()
    {
        PlayerActions.Enable();
    }
}
