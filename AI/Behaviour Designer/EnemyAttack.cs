using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

public class EnemyAttack : Action
{
    public Animator anim;
    public SharedTransform playerTransform;
    public SharedBool isAttacking;

    private float trackingSpeed = 10f; // ���� �� ȸ�� �ӵ�
    private float rotateRange = 60; // ���� �� ȸ�� ���� ����

    public override TaskStatus OnUpdate()
    {
        // �÷��̾�� ���� ���� ���
        Vector3 directionToPlayer = (playerTransform.Value.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z));

        // ���� ���� �ٶ󺸰� �ִ� ����� �÷��̾� �� ������ ���� ���� ���
        float angleToPlayer = Quaternion.Angle(transform.rotation, targetRotation);

        if (angleToPlayer < rotateRange)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, trackingSpeed);
        }

        anim.SetTrigger("EnemyAttack");
        isAttacking.Value = false;

        return TaskStatus.Success;
    }
}
