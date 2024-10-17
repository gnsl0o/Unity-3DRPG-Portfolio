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

        // 씬에서 "Player"라는 이름의 오브젝트를 찾기
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
            // 타겟의 방향 벡터를 계산
            Vector3 directionToTarget = playerTransform.position - transform.position;
            directionToTarget.y = 0; // 수직 방향의 회전은 무시 (2D 평면에서 회전)

            // 방향 벡터를 기준으로 회전 생성
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget.normalized);

            // 부드러운 회전 적용
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }
}
