using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;

public class EnemyPatrol : Action
{
    public NavMeshAgent agent;
    public SharedTransform playerTransform;
    public Animator anim;
    public SharedBool isChasing;

    private float updateInterval = 1.0f; // 경로 업데이트 주기 (초)
    private float timeSinceLastUpdate = 0f;

    public override void OnStart()
    {
        agent.SetDestination(playerTransform.Value.position);
    }

    public override TaskStatus OnUpdate()
    {
        timeSinceLastUpdate += Time.deltaTime;

        if (timeSinceLastUpdate >= updateInterval)
        {
            SetAgentDestination();
            timeSinceLastUpdate = 0f;
        }


        

        Vector3 direction = agent.desiredVelocity;

        anim.SetFloat("MoveSpeed", direction.magnitude);

        // transform.position = agent.nextPosition;

        // 직접 거리 계산
        float distanceToTarget = Vector3.Distance(transform.position, playerTransform.Value.position);

        // 목표와의 거리 기반으로 상태 반환
        if (distanceToTarget < agent.stoppingDistance)
        {
            anim.SetFloat("MoveSpeed", 0);
            return TaskStatus.Success;
        }

        // 너무 멀어지면 Failure 반환
        if (agent.remainingDistance > 30f)
        {
            anim.SetFloat("MoveSpeed", 0);

            Debug.Log("너무 멀어서 더이상 쫒지 않음");

            isChasing.Value = false;

            return TaskStatus.Failure;
        }

        return TaskStatus.Running;
    }

    private void SetAgentDestination()
    {
        if (agent.destination != playerTransform.Value.position)
        {
            agent.SetDestination(playerTransform.Value.position);
        }
    }
}