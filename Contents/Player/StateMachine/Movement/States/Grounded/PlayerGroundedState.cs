using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundedState : PlayerMovementState
{
    private SlopeData slopeData;

    public PlayerGroundedState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
        slopeData = stateMachine.player.ColliderUtility.SlopeData;
    }

    #region IState Methods

    public override void EnterState()
    {
        base.EnterState();

        StartAnimation(stateMachine.player.AnimationsData.GroundedParameterHash);
    }

    public override void ExitState()
    {
        base.ExitState();

        StopAnimation(stateMachine.player.AnimationsData.GroundedParameterHash);
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        Float();
    }
    #endregion

    #region Main Methods

    private void Float()
    {
        Vector3 capsuleColliderCenterInWorldSpace = stateMachine.player.ColliderUtility.CapsuleColliderData.Collider.bounds.center;

        Ray downwardsRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWorldSpace, Vector3.down);

        if (Physics.Raycast(downwardsRayFromCapsuleCenter, out RaycastHit hit, slopeData.FloatRayDistacne,stateMachine.player.LayerData.GroundLayer,QueryTriggerInteraction.Ignore))
        {
            float groundAngle = Vector3.Angle(hit.normal, -downwardsRayFromCapsuleCenter.direction);

            float slopeSpeedModifier = SetSlopSpeedModifierOnAngle(groundAngle);

            if (slopeSpeedModifier == 0f)
            {
                return;
            }

            float distanceToFloatingPoint = stateMachine.player.ColliderUtility.CapsuleColliderData.ColliderCenterInLocalSpace.y * stateMachine.player.transform.localScale.y - hit.distance; 

            if(distanceToFloatingPoint == 0f)
            {
                return;
            }

            float amountToLift = distanceToFloatingPoint * slopeData.StepReachForce - GetPlayerVerticalVelocity().y;

            Vector3 liftForce = new Vector3(0f, amountToLift, 0f);

            stateMachine.player.Rigidbody.AddForce(liftForce, ForceMode.VelocityChange);
        }
    }

    private float SetSlopSpeedModifierOnAngle(float angle)
    {
        float slopeSpeedModifier = movementData.SlopeSpeedAngles.Evaluate(angle);

        stateMachine.ReusableData.MovementOnSlopesSpeedModifier = slopeSpeedModifier;

        return slopeSpeedModifier;
    }

    #endregion

    #region Resuable Methods
    protected override void AddInputActionsCallbacks()
    {
        base.AddInputActionsCallbacks();

        stateMachine.player.Input.PlayerActions.Movement.canceled += OnMovementCanceled;

        stateMachine.player.Input.PlayerActions.Dash.started += OnDashStarted;
    }
    protected override void RemoveinputActionsCallback()
    {
        base.RemoveinputActionsCallback();

        stateMachine.player.Input.PlayerActions.Movement.canceled -= OnMovementCanceled;

        stateMachine.player.Input.PlayerActions.Dash.started -= OnDashStarted;
    }
    protected virtual void OnMove()
    {
        if (stateMachine.ReusableData.ShouldWalk)
        {
            stateMachine.SetState(stateMachine.WalkingState);

            return;
        }

        stateMachine.SetState(stateMachine.RunningState);
    }

    #endregion

    #region Input Methods
    protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
    {
        stateMachine.SetState(stateMachine.IdlingState);
    }
    protected virtual void OnDashStarted(InputAction.CallbackContext context)
    {
        stateMachine.SetState(stateMachine.DashingState);
    }
    #endregion
}
