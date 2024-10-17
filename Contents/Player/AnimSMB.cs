using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimSMB : StateMachineBehaviour
{
    PlayerContext playerContext;
    private bool isPlayerCached = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(playerContext == null)
        {
            playerContext = animator.gameObject.GetComponent<PlayerContext>();
            isPlayerCached=true;
        }

        playerContext.actionStateMachine.SetState(playerContext.actionStateMachine.WeaponDrawnState);
    }
}
