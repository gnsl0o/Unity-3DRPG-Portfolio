using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableMovementOnStateExit : StateMachineBehaviour
{
    private PlayerContext playerContext;
    private bool isControllerCashed = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        if(!isControllerCashed)
        {
            playerContext = animator.GetComponent<PlayerContext>();
            isControllerCashed = true;
        }
    }
}