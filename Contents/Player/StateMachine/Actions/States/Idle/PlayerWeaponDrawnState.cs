using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponDrawnState : PlayerAction
{
    public PlayerWeaponDrawnState(PlayerActionStateMachine playerActionStateMachine)
    {
        stateMachine = playerActionStateMachine;
    }

    public override void EnterState()
    {
        stateMachine.player.Input.SwitchActionMap("Player");
        stateMachine.player.Input.SwitchActionMap("Battle");
    }
}