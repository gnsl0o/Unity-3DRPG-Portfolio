using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleState : PlayerAction
{
    public override void EnterState()
    {
        
    }

    public override void ExitState() { }

    public BattleState(PlayerActionStateMachine playerActionStateMachine)
    {
        stateMachine = playerActionStateMachine;
    }
}
