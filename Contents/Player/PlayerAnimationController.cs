using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private PlayerContext playerContext;
    public bool rotateOn = false;

    private void Start()
    {
        playerContext = GetComponent<PlayerContext>();
    }

    private void FixedUpdate()
    {
        if (rotateOn)
        {
            RotatePlayer();
        }
    }

    public void RotatePlayer()
    {
        if(playerContext.actionStateMachine.ReusableData.Direction.sqrMagnitude > 0.01f)
        {
            float targetAngle = Mathf.Atan2(playerContext.actionStateMachine.ReusableData.Direction.x, playerContext.actionStateMachine.ReusableData.Direction.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            playerContext.Rigidbody.rotation = Quaternion.Slerp(playerContext.Rigidbody.rotation, targetRotation, Time.deltaTime * 5); // 5는 회전 속도 파라미터
        }
    }

    public void EnableWeaponDrawnState()
    {
        if (playerContext.actionStateMachine.CurrentState != playerContext.actionStateMachine.WeaponDrawnState)
        {
            playerContext.actionStateMachine.SetState(playerContext.actionStateMachine.WeaponDrawnState); // 애니메이션이 끝나면 WeaponDrawnState로 전환
            playerContext.Player.movementStateMachine.SetState(playerContext.Player.movementStateMachine.IdlingState);
            playerContext.anim.SetBool(playerContext.AnimationsData.MovingParameterHash, false);
        }
    }

    public void EnableWeaponSheathedState()
    {
        if (playerContext.actionStateMachine.CurrentState != playerContext.actionStateMachine.SheathedState)
        {
            Debug.Log("WeaponSheathedState 상태 진입");
            playerContext.actionStateMachine.SetState(playerContext.actionStateMachine.SheathedState);    
        }
    }

    // InputBuffer로부터 마지막 입력을 받아와 다음 동작을 결정
    public void MoveNextAnimation()
    {
        string animName = playerContext.inputBuffer.GetLastInput();

        Debug.Log("마지막 인풋 : " + animName);

        switch(animName)
        {
            case "Attack": 
                playerContext.anim.SetTrigger("OnCloseAttackCombo");
                break;
            case "Parry":
                playerContext.anim.ResetTrigger("OnCloseAttackCombo");
                playerContext.actionStateMachine.SetState(playerContext.actionStateMachine.DefenseState);
                playerContext.parry.EnableParry();
                playerContext.anim.SetBool("IsParrying", true);
                break;
        }

        playerContext.inputBuffer.ClearLastInput();
    }
    

    public void EnableRotation()
    {
        rotateOn = true;
    }

    public void DisableRotation()
    {
        rotateOn = false;
    }
}
