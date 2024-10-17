using UnityEngine;

public class PlayerAttackState : BattleState
{
    public PlayerAttackState(PlayerActionStateMachine playerActionStateMachine) : base(playerActionStateMachine)
    {
    }

    public override void EnterState()
    {
        stateMachine.player.Input.SwitchActionMap("Attack");
        stateMachine.player.parry.DisableParry();
        //stateMachine.player.anim.SetBool("IsParrying", false);
    }

    public override void ExitState()
    {
        stateMachine.player.parry.EnableParry();
        stateMachine.player.attackStrategy.ResetAttackCount();
        stateMachine.player.attackStrategy.EnableInput();
    }

    public override void Update()
    {
        ReadMovementInput();
    }

    public override void PhysicsUpdate()
    {
        RotatePlayer();
    }

    private void RotatePlayer()
    {
        Vector3 movementDirection = GetMovementInputDirection();

        stateMachine.ReusableData.Direction = movementDirection;
    }

    protected Vector3 GetMovementInputDirection()
    {
        Vector2 input = stateMachine.ReusableData.MovementInput;
        Vector3 forward = stateMachine.player.MainCameraTransform.forward;
        Vector3 right = stateMachine.player.MainCameraTransform.right;

        forward.y = 0; // 카메라의 수평 방향만 고려
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        return forward * input.y + right * input.x;
    }
    void ReadMovementInput()
    {
        stateMachine.ReusableData.MovementInput = stateMachine.player.Input.AttackActions.Rotate.ReadValue<Vector2>();
    }
}
