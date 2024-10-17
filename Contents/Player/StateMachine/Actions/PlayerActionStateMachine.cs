using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionStateMachine : StateMachine
{
    public PlayerContext player { get; }
    public PlayerStateReusableData ReusableData { get; }
    public BattleState BattleState { get; }
    public PlayerWeaponDrawnState WeaponDrawnState { get; }
    public PlayerWeaponSheathedState SheathedState { get; }
    public PlayerConversationState ConversationState { get; }
    public PlayerAttackState AttackState { get; }  
    public PlayerDefenseState DefenseState { get; }

    public PlayerActionStateMachine(PlayerContext player)
    {
        this.player = player;
        ReusableData = new PlayerStateReusableData();

        BattleState = new BattleState(this);

        WeaponDrawnState = new PlayerWeaponDrawnState(this);

        SheathedState = new PlayerWeaponSheathedState(this);

        ConversationState = new PlayerConversationState(this);

        AttackState = new PlayerAttackState(this);
        DefenseState = new PlayerDefenseState(this);
    }
}
