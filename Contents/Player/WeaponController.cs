using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{
    private Animator animator;
    private bool isWeaponDrawn = false; // ���� ���¸� �����ϴ� bool ����
    public PlayerContext playerContext; // PlayerContext�� ���� public ����

    private InputAction weaponSwitch;

    private void Start()
    {
        animator = GetComponent<Animator>();

        weaponSwitch = playerContext.Input.PlayerActions.WeaponSwitch;
        weaponSwitch.performed += _ => WeaponSwitch();
    }

    private void WeaponSwitch()
    {
        if (playerContext.actionStateMachine.CurrentState != playerContext.actionStateMachine.AttackState)
        {
            // ���� ���¿� ���� �ִϸ��̼� Ʈ���� ����
            if (isWeaponDrawn)
            {
                playerContext.actionStateMachine.SetState(playerContext.actionStateMachine.SheathedState);
                animator.SetTrigger("DrawWeapon"); // ���⸦ �ִ� �ִϸ��̼�
                isWeaponDrawn = false; // ���� ������Ʈ
            }
            else
            {
                playerContext.actionStateMachine.SetState(playerContext.actionStateMachine.WeaponDrawnState);
                animator.SetTrigger("DrawWeapon"); // ���⸦ �ִ� �ִϸ��̼�
                isWeaponDrawn = true; // ���� ������Ʈ
            }
        }
    }
}