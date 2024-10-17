using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefenseState : BattleState
{
    public PlayerDefenseState(PlayerActionStateMachine playerActionStateMachine) : base(playerActionStateMachine)
    {
        
    }

    public override void EnterState()
    {
        base.EnterState();
        stateMachine.player.Input.EnableDefenseState();
    }
}
