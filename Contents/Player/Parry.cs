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
        Debug.Log("ParryPerformed() ȣ��");
        if (canParry)
        {
            playerContext.actionStateMachine.SetState(playerContext.actionStateMachine.DefenseState);
            Debug.Log("��� ����");
            anim.SetBool("IsParrying", true);
        }
    }

    private void ParryCanceled()
    {
        Debug.Log("��� ���");
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
