using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponSheathedState : PlayerAction
{
    public PlayerWeaponSheathedState(PlayerActionStateMachine playerActionStateMachine)
    {
        stateMachine = playerActionStateMachine;
    }
    public override void EnterState()
    {
        stateMachine.player.Input.SwitchActionMap("Player");
        stateMachine.player.interaction.UpdateDialogueState(true);
    }
}
