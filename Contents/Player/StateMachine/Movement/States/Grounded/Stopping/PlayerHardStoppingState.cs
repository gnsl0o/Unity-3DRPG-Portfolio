using UnityEngine;

public class PlayerHardStoppingState : PlayerStoppingState
{
    public PlayerHardStoppingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }
    #region IState Methods

    public override void EnterState()
    {
        base.EnterState();

        StartAnimation(stateMachine.player.AnimationsData.HardStopParameterHash);

        stateMachine.ReusableData.MovementDecelerationForce = movementData.StopData.HardDecelerationForce;
    }

    public override void ExitState()
    {
        base.ExitState();

        StopAnimation(stateMachine.player.AnimationsData.HardStopParameterHash);
    }

    #endregion

    #region Reusalbe Methods
    protected override void OnMove()
    {
        base.OnMove();

        if (stateMachine.ReusableData.ShouldWalk)
        {
            return;
        }

        stateMachine.SetState(stateMachine.RunningState);
    }

    #endregion
}
