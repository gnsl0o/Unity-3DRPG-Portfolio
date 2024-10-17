using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRunningState : PlayerMovingState
{
    private PlayerSprintData sprintData;

    private float startTime;

    public PlayerRunningState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
        sprintData = movementData.SprintData;
    }

    #region IState Methods
    public override void EnterState()
    {
        base.EnterState();

        StartAnimation(stateMachine.player.AnimationsData.RunParameterHash);

        stateMachine.ReusableData.MovementSpeedModifier = movementData.RunData.SpeedModifier;

        startTime = Time.time;
    }

    public override void ExitState()
    {
        base.ExitState();

        StopAnimation(stateMachine.player.AnimationsData.RunParameterHash);
    }

    public override void Update()
    {
        base.Update();

        if (!stateMachine.ReusableData.ShouldWalk)
        {
            return;
        }

        if(Time.time < startTime + sprintData.RunToWalkTime)
        {
            return;
        }

        StopRunning();
    }

    #endregion

    #region Main Methods

    private void StopRunning()
    {
        if(stateMachine.ReusableData.MovementInput == Vector2.zero)
        {
            stateMachine.SetState(stateMachine.IdlingState);

            return;
        }

        stateMachine.SetState(stateMachine.WalkingState);
    }

    #endregion

    #region Resuable Methods
    protected override void AddInputActionsCallbacks()
    {
        base.AddInputActionsCallbacks();
        stateMachine.player.Input.PlayerActions.Movement.canceled += OnMovementCanceled;
    }

    protected override void RemoveinputActionsCallback()
    {
        base.RemoveinputActionsCallback();
        stateMachine.player.Input.PlayerActions.Movement.canceled -= OnMovementCanceled;
    }
    #endregion

    #region Input Methods
    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        if (stateMachine.player.playerContext.actionStateMachine.CurrentState != stateMachine.player.playerContext.actionStateMachine.AttackState)
        {
            stateMachine.SetState(stateMachine.MediumStoppingState);
        }
    }
    protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
    {
        base.OnWalkToggleStarted(context);

        stateMachine.SetState(stateMachine.WalkingState);
    }
    #endregion
}
