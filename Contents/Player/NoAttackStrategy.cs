using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoAttackStrategy : MonoBehaviour, IAttackStrategy
{
    public void Attack()
    {
        Debug.Log("���� �Ұ�");
    }
   
    public void walk()
    {

    }

    public void EnableInput()
    {

    }

    // �ִϸ��̼� �̺�Ʈ�� ȣ��� �޼���
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
