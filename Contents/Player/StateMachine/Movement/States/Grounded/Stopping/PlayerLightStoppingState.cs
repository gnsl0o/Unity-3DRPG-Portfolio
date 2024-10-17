using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLightStoppingState : PlayerStoppingState
{
    public PlayerLightStoppingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }
    #region IState Methods

    public override void EnterState()
    {
        base.EnterState();

        stateMachine.ReusableData.MovementDecelerationForce = movementData.StopData.LightDecelerationForce;
    }

    #endregion
}
