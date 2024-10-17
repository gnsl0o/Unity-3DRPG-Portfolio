using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Parry : MonoBehaviour
{
    public AudioClip bgm;

    private Animator anim;
    public PlayerContext playerContext;

    private bool canParry = true;

    private void Start()
    {
        anim = GetComponent<Animator>();

        playerContext.Input.AttackActions.Parry.performed += _ => ParryPerformed();
        playerContext.Input.AttackActions.Parry.canceled += _ => ParryCanceled();

        playerContext.Input.BattleActions.Parry.performed += _ => ParryPerformed();
        playerContext.Input.BattleActions.Parry.canceled += _ => ParryCanceled();
    }
    private void ParryPerformed()
    {
        Debug.Log("ParryPerformed() 호출");
        if (canParry)
        {
            playerContext.actionStateMachine.SetState(playerContext.actionStateMachine.DefenseState);
            Debug.Log("방어 상태");
            anim.SetBool("IsParrying", true);
        }
    }

    private void ParryCanceled()
    {
        Debug.Log("방어 취소");
        anim.SetBool("IsParrying", false);
    }

    public void EnableParry()
    {
        canParry = true;
    }

    public void DisableParry()
    {
        canParry = false;
    }
}
