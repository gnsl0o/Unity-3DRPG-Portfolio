using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStoppingState : PlayerGroundedState
{
    public PlayerStoppingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }
    #region IState Methods
    
    public override void EnterState()
    {
        base.EnterState();

        StartAnimation(stateMachine.player.AnimationsData.StoppingParameterHash);

        stateMachine.ReusableData.MovementSpeedModifier = 0f;
    }

    public override void ExitState()
    {
        base.ExitState();

        StopAnimation(stateMachine.player.AnimationsData.StoppingParameterHash);
    }

    #endregion

    #region Main Methods
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        RotateTowardsTargetRoatiton();

        if (isMovingHorizontally())
        {
            return;
        }

        DecelerateHorizontally();
    }

    public override void OnAnimationTransitionEvent()
    {
        stateMachine.SetState(stateMachine.IdlingState);
    }

    #endregion

    #region Reusalbe Methods

    protected override void AddInputActionsCallbacks()
    {
        base.AddInputActionsCallbacks();

        stateMachine.player.Input.PlayerActions.Movement.started += OnMovementStarted;
    }

    protected override void RemoveinputActionsCallback()
    {
        base.RemoveinputActionsCallback();

        stateMachine.player.Input.PlayerActions.Movement.started -= OnMovementStarted;
    }

    #endregion

    #region Input Methods
    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
    }
    private void OnMovementStarted(InputAction.CallbackContext context)
    {
        OnMove();
    }

    #endregion
}
