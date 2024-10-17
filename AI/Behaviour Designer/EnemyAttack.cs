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

    private float trackingSpeed = 10f; // 공격 시 회전 속도
    private float rotateRange = 60; // 공격 시 회전 가능 범위

    public override TaskStatus OnUpdate()
    {
        // 플레이어와 적의 방향 계산
        Vector3 directionToPlayer = (playerTransform.Value.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z));

        // 적이 현재 바라보고 있는 방향과 플레이어 쪽 방향의 각도 차이 계산
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
