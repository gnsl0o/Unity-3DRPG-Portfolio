using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    public FieldOfView fov; // FieldOfView ��ũ��Ʈ�� ����
    private NavMeshAgent agent;
    Animator anim;

    private bool isAttacking = false; // ���� ������ ����
    private float attackDelay = 5f; // ���� ���� (��)
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
            Debug.Log("���� ���� �߰�");
            agent.SetDestination(fov.visibleTargets[0].position);
        }

        // �÷��̾ �߰��ߴٸ�
        if (fov.visibleTargets.Count > 0)
        {
            // NavMeshAgent�� ��ǥ������ �÷��̾��� ��ġ�� ����
            agent.SetDestination(fov.visibleTargets[0].position);

            // �÷��̾�� ����� ���������, ���� ���� �ƴ϶�� ����
            if (agent.remainingDistance <= 1f && !isAttacking)
            {
                StartCoroutine(Attack());
            }
        }
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.5f); // ��� ����� ���� �̼��� ������

        isAttacking = true; // ���� ������ ����
        anim.SetBool("ZombieAttack", true); // ���� �ִϸ��̼� ����
        agent.isStopped = true; // ������Ʈ �̵� ����

        yield return new WaitForSeconds(attackAnimationLength); // ���� �ִϸ��̼��� ���̸�ŭ ���

        anim.SetBool("ZombieAttack", false); // ���� �ִϸ��̼� ����
        agent.isStopped = false; // ������Ʈ �̵� ����

        yield return new WaitForSeconds(attackDelay - attackAnimationLength); // ���� ���� - ���� �ִϸ��̼��� ���̸�ŭ ���

        isAttacking = false; // ���� ���� �ƴ����� ����
    }
}
