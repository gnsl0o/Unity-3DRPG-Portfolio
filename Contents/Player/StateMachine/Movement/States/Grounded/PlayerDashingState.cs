using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDashingState : PlayerGroundedState
{
    private PlayerDashData dashData;

    private float startTime;

    private int consecutiveDashesUesd;

    private bool shouldKeepRotationg;

    public PlayerDashingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
        dashData = new PlayerDashData();
    }
    #region IState Methods
    public override void EnterState()
    {
        base.EnterState();
        
        StartAnimation(stateMachine.player.AnimationsData.DashParameterHash);

        stateMachine.ReusableData.MovementSpeedModifier = dashData.SpeedModifier;

        stateMachine.ReusableData.RotationData = dashData.RotationData;

        AddForceOnTransitionFromStationaryState();

        shouldKeepRotationg = stateMachine.ReusableData.MovementInput != Vector2.zero;

        UpdateConsecutiveDashes();

        startTime = Time.time;
    }

    public override void ExitState()
    {
        base.ExitState();

        StopAnimation(stateMachine.player.AnimationsData.DashParameterHash);

        SetBaseRotationData();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (!shouldKeepRotationg)
        {
            return;
        }

        RotateTowardsTargetRoatiton();
    }

    public override void OnAnimationTransitionEvent()
    {
        if(stateMachine.ReusableData.MovementInput == Vector2.zero)
        {
            stateMachine.SetState(stateMachine.HardStoppingState);

            return;
        }

        stateMachine.SetState(stateMachine.SprintingState);
    }

    #endregion

    #region Main Methods
    private void AddForceOnTransitionFromStationaryState()
    {
        if(stateMachine.ReusableData.MovementInput != Vector2.zero)
        {
            return;
        }

        Vector3 characterRotationDirection = stateMachine.player.transform.forward;

        characterRotationDirection.y = 0;

        UpdateTargetRotation(characterRotationDirection, false);

        stateMachine.player.Rigidbody.velocity = characterRotationDirection * GetMovementSpeed();
    }
    private void UpdateConsecutiveDashes()
    {
        if (!IsConsecutive())
        {
            consecutiveDashesUesd = 0;
        }
        
        ++consecutiveDashesUesd;

        if(consecutiveDashesUesd == dashData.ConsecutiveDashesLimitAmount)
        {
            consecutiveDashesUesd = 0;

            stateMachine.player.Input.DisableActionFor(stateMachine.player.Input.PlayerActions.Dash, dashData.DashLimitReachedCooldown);
        }
    }

    private bool IsConsecutive()
    {
        return Time.time < startTime - dashData.TimeToBeConsideredConsecutive;
    }
    #endregion

    #region Reusable Methods

    protected override void AddInputActionsCallbacks()
    {
        base.AddInputActionsCallbacks();

        stateMachine.player.Input.PlayerActions.Movement.performed += OnMovementPerformed;
    }

    protected override void RemoveinputActionsCallback()
    {
        base.RemoveinputActionsCallback();

        stateMachine.player.Input.PlayerActions.Movement.performed -= OnMovementPerformed;
    }

    #endregion

    #region Input Mehtods
    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
    }

    protected override void OnDashStarted(InputAction.CallbackContext context)
    {
    }
    private void OnMovementPerformed(InputAction.CallbackContext context)
    {
        shouldKeepRotationg = true;
    }
    #endregion
}
