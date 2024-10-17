using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEventTrigger : MonoBehaviour
{
    private Playermovement player;

    private void Awake()
    {
        player = GetComponent<Playermovement>();
    }

    public void TriggerOnMovementStateAnimationEnterEvent()
    {
        player.OnMovementStateAnimationEnterEvent();
    }

    public void TriggerOnMovementStateAnimationExitEvent()
    {
        player.OnMovementStateAnimationExitEvent();
    }

    public void TriggerOnMovementStateAnimationTransitionEvent()
    {
        player.OnMovementStateAnimationTransitionEvent();
    }
}
