using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWalkingState : PlayerMovingState
{
    public PlayerWalkingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
        
    }

    #region IState Methods
    public override void EnterState()
    {
        base.EnterState();

        StartAnimation(stateMachine.player.AnimationsData.WalkParameterHash);

        stateMachine.ReusableData.MovementSpeedModifier = movementData.WalkData.SpeedModifier;
    }

    public override void ExitState()
    {
        base.ExitState();

        StopAnimation(stateMachine.player.AnimationsData.WalkParameterHash);
    }
    #endregion

    #region Input Methods

    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        if (stateMachine.player.playerContext.actionStateMachine.CurrentState != stateMachine.player.playerContext.actionStateMachine.AttackState)
        {
            stateMachine.SetState(stateMachine.LightStoppingState);
        }
    }
    protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
    {
        base.OnWalkToggleStarted(context);

        stateMachine.SetState(stateMachine.RunningState);
    }
    #endregion
}
