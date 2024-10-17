using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : IState
{
    protected PlayerActionStateMachine stateMachine;

    public virtual void EnterState()
    {
    }

    public virtual void ExitState()
    {
    }

    public virtual void HandleInput()
    {
    }

    public virtual void OnAnimationEnterEvent()
    {
    }

    public virtual void OnAnimationExitEvent()
    {
    }

    public void OnAnimationTransitionEvent()
    {
    }

    public virtual void PhysicsUpdate()
    {
    }

    public virtual void Update()
    {
    }

    protected void StartAnimation(int animationHash)
    {
        stateMachine.player.Animator.SetBool(animationHash, true);
    }
    protected void StopAnimation(int animationHash)
    {
        stateMachine.player.Animator.SetBool(animationHash, false);
    }
}
