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
            playerContext.Rigidbody.rotation = Quaternion.Slerp(playerContext.Rigidbody.rotation, targetRotation, Time.deltaTime * 5); // 5�� ȸ�� �ӵ� �Ķ����
        }
    }

    public void EnableWeaponDrawnState()
    {
        if (playerContext.actionStateMachine.CurrentState != playerContext.actionStateMachine.WeaponDrawnState)
        {
            playerContext.actionStateMachine.SetState(playerContext.actionStateMachine.WeaponDrawnState); // �ִϸ��̼��� ������ WeaponDrawnState�� ��ȯ
            playerContext.Player.movementStateMachine.SetState(playerContext.Player.movementStateMachine.IdlingState);
            playerContext.anim.SetBool(playerContext.AnimationsData.MovingParameterHash, false);
        }
    }

    public void EnableWeaponSheathedState()
    {
        if (playerContext.actionStateMachine.CurrentState != playerContext.actionStateMachine.SheathedState)
        {
            Debug.Log("WeaponSheathedState ���� ����");
            playerContext.actionStateMachine.SetState(playerContext.actionStateMachine.SheathedState);    
        }
    }

    // InputBuffer�κ��� ������ �Է��� �޾ƿ� ���� ������ ����
    public void MoveNextAnimation()
    {
        string animName = playerContext.inputBuffer.GetLastInput();

        Debug.Log("������ ��ǲ : " + animName);

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
