using BehaviorDesigner.Runtime;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyContext : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;

    public SharedBool isChasing;
    public SharedBool isAttacking;
    public Transform playerTransform;
    public SharedTransform playerTransformLocal;

    private BehaviorTree behaviorTree;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        agent.updatePosition = false;
        agent.updateRotation = false;

        // Get the BehaviorTree component attached to the same GameObject
        behaviorTree = GetComponent<BehaviorTree>();

        // ������ "Player"��� �̸��� ������Ʈ�� ã��
        playerTransform = GameObject.FindWithTag("Player").transform;
        playerTransformLocal = (SharedTransform)behaviorTree.GetVariable("Player");
        playerTransformLocal.Value = playerTransform;

        isChasing = (SharedBool)behaviorTree.GetVariable("isChasing");
        isAttacking = (SharedBool)behaviorTree.GetVariable("isAttacking");
    }


    private void Update()
    {
        if (isChasing.Value && !isAttacking.Value)
        {
            // Ÿ���� ���� ���͸� ���
            Vector3 directionToTarget = playerTransform.position - transform.position;
            directionToTarget.y = 0; // ���� ������ ȸ���� ���� (2D ��鿡�� ȸ��)

            // ���� ���͸� �������� ȸ�� ����
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget.normalized);

            // �ε巯�� ȸ�� ����
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }
}
