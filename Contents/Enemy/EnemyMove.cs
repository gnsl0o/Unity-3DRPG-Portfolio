using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    public FieldOfView fov; // FieldOfView 스크립트를 참조
    private NavMeshAgent agent;
    Animator anim;

    private bool isAttacking = false; // 공격 중인지 여부
    private float attackDelay = 5f; // 공격 간격 (초)
    private float attackAnimationLength = 2f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (fov.visibleTargets.Count > 0)
        {
            Debug.Log("적이 나를 발견");
            agent.SetDestination(fov.visibleTargets[0].position);
        }

        // 플레이어를 발견했다면
        if (fov.visibleTargets.Count > 0)
        {
            // NavMeshAgent의 목표지점을 플레이어의 위치로 설정
            agent.SetDestination(fov.visibleTargets[0].position);

            // 플레이어에게 충분히 가까워졌고, 공격 중이 아니라면 공격
            if (agent.remainingDistance <= 1f && !isAttacking)
            {
                StartCoroutine(Attack());
            }
        }
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.5f); // 경로 계산을 위한 미세한 딜레이

        isAttacking = true; // 공격 중으로 설정
        anim.SetBool("ZombieAttack", true); // 공격 애니메이션 실행
        agent.isStopped = true; // 에이전트 이동 중지

        yield return new WaitForSeconds(attackAnimationLength); // 공격 애니메이션의 길이만큼 대기

        anim.SetBool("ZombieAttack", false); // 공격 애니메이션 중지
        agent.isStopped = false; // 에이전트 이동 시작

        yield return new WaitForSeconds(attackDelay - attackAnimationLength); // 공격 간격 - 공격 애니메이션의 길이만큼 대기

        isAttacking = false; // 공격 중이 아님으로 설정
    }
}
