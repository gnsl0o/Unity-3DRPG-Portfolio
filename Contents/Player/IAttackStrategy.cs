using Unity.VisualScripting;
using UnityEngine;
public interface IAttackStrategy
{
    public void Attack();

    public void EnableInput();

    // �ִϸ��̼� �̺�Ʈ�� ȣ��� �޼���
    public void DisableInput();

    public void ResetAttackCount();
}