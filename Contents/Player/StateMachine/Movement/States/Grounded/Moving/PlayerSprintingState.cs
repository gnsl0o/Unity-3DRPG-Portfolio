using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSprintState : PlayerMovingState
{
    private PlayerSprintData sprintData;

    private float startTime;

    private bool keepSprinting;
    public PlayerSprintState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
        sprintData = movementData.SprintData;
    }

    #region IState Methods

    public override void EnterState()
    {
        base.EnterState();

        StartAnimation(stateMachine.player.AnimationsData.SprintParameterHash);

        stateMachine.ReusableData.MovementSpeedModifier = sprintData.SpeedModifier;

        startTime = Time.time;
    }

    public override void ExitState()
    {
        base.ExitState();

        StopAnimation(stateMachine.player.AnimationsData.SprintParameterHash);

        keepSprinting = false;
    }

    public override void Update()
    {
        base.Update();

        if (keepSprinting)
        {
            return;
        }

        if (Time.time < startTime + sprintData.SpeedToRunTime)
        {
            return;
        }

        StopSprinting();
    }

    #endregion

    #region Main Methods

    private void StopSprinting()
    {
        if(stateMachine.ReusableData.MovementInput == Vector2.zero)
        {
            stateMachine.SetState(stateMachine.IdlingState);

            return;
        }

        stateMachine.SetState(stateMachine.RunningState);
    }

    #endregion

    #region Reusable Methods

    protected override void AddInputActionsCallbacks()
    {
        base.AddInputActionsCallbacks();

        stateMachine.player.Input.PlayerActions.Sprint.performed += OnSprintPerformed;
    }

    protected override void RemoveinputActionsCallback()
    {
        base.RemoveinputActionsCallback();

        stateMachine.player.Input.PlayerActions.Sprint.performed -= OnSprintPerformed;
    }

    #endregion

    #region Input Methods

    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        if(stateMachine.player.playerContext.actionStateMachine.CurrentState != stateMachine.player.playerContext.actionStateMachine.AttackState)
        {
            stateMachine.SetState(stateMachine.HardStoppingState);
        }
    }

    private void OnSprintPerformed(InputAction.CallbackContext context)
    {
        keepSprinting = true;
    }

    #endregion
}
