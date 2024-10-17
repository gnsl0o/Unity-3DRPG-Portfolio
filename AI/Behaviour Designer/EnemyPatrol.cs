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

    private float updateInterval = 1.0f; // ��� ������Ʈ �ֱ� (��)
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

        // ���� �Ÿ� ���
        float distanceToTarget = Vector3.Distance(transform.position, playerTransform.Value.position);

        // ��ǥ���� �Ÿ� ������� ���� ��ȯ
        if (distanceToTarget < agent.stoppingDistance)
        {
            anim.SetFloat("MoveSpeed", 0);
            return TaskStatus.Success;
        }

        // �ʹ� �־����� Failure ��ȯ
        if (agent.remainingDistance > 30f)
        {
            anim.SetFloat("MoveSpeed", 0);

            Debug.Log("�ʹ� �־ ���̻� �i�� ����");

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