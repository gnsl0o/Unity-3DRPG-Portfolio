using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{
    private Animator animator;
    private bool isWeaponDrawn = false; // 무기 상태를 추적하는 bool 변수
    public PlayerContext playerContext; // PlayerContext에 대한 public 참조

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
            // 무기 상태에 따라 애니메이션 트리거 설정
            if (isWeaponDrawn)
            {
                playerContext.actionStateMachine.SetState(playerContext.actionStateMachine.SheathedState);
                animator.SetTrigger("DrawWeapon"); // 무기를 넣는 애니메이션
                isWeaponDrawn = false; // 상태 업데이트
            }
            else
            {
                playerContext.actionStateMachine.SetState(playerContext.actionStateMachine.WeaponDrawnState);
                animator.SetTrigger("DrawWeapon"); // 무기를 넣는 애니메이션
                isWeaponDrawn = true; // 상태 업데이트
            }
        }
    }
}