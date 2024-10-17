using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoAttackStrategy : MonoBehaviour, IAttackStrategy
{
    public void Attack()
    {
        Debug.Log("공격 불가");
    }
   
    public void walk()
    {

    }

    public void EnableInput()
    {

    }

    // 애니메이션 이벤트로 호출될 메서드
    public void DisableInput()
    {

    }

    public void ResetAttackCount()
    {

    }

    public void EnableAttackInput()
    {
        throw new System.NotImplementedException();
    }

    public void DisableAttackInput()
    {
        throw new System.NotImplementedException();
    }
}
