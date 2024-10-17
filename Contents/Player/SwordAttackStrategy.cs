using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class SwordAttackStrategy : MonoBehaviour, IAttackStrategy
{
    private Animator anim;
    public GameObject weapon;

    public PlayerContext playerContext;

    private bool canInput = true; // �÷��̾��� �Է��� ���� �� �ִ��� ����

    public float attackRange = 10f;

    private int attackCount = 0;

    void Start()
    {
        playerContext = GetComponent<PlayerContext>();

        anim = playerContext.anim;

        playerContext.Input.BattleActions.Attack.performed += OnAttackPerformed;
        playerContext.Input.AttackActions.Attack.performed += OnAttackPerformed;

        // attackAction.Enable();
    }

    public void Attack()
    {
        if (anim != null)
        {
            anim.SetTrigger("OnCloseAttackCombo");
        }
    }

    // �ִϸ��̼� �̺�Ʈ�� ȣ��� �޼���
    public void EnableInput()
    {
        canInput = true;
    }

    // �ִϸ��̼� �̺�Ʈ�� ȣ��� �޼���
    public void DisableInput()
    {
        canInput = false;
    }

    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        // �Է��� ���� �� �ִ� �����̰�, �÷��̾ ���� ��ư�� ������ ���
        if (canInput)
        {
            if (playerContext.actionStateMachine.CurrentState is not PlayerAttackState)
            {
                playerContext.actionStateMachine.SetState(playerContext.actionStateMachine.AttackState);
            }

            if (attackCount == 0 && playerContext.Player.movementStateMachine.CurrentState == playerContext.Player.movementStateMachine.IdlingState)
            {
                CheckForParryAttack();
                DisableInput();
            }
            else
            {
                Attack();
                DisableInput();
            }
        }
    }

    void CheckForParryAttack()
    {
        RaycastHit hit;
        // ����ĳ��Ʈ�� �������� ���̱� ���� y �� ���� ����
        Vector3 startPosition = transform.position + new Vector3(0, 1.0f, 0); // ��: �����߽ɺ��� 1.0 ���� ���� ����
        Vector3 forward = transform.forward;

        if (Physics.Raycast(startPosition, forward, out hit, attackRange, 1 << 6))
        {
            EnemyHealth enemy = hit.collider.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                // ����ĳ��Ʈ�� ���� ���� �и� �������� ���� ���ִ��� Ȯ��
                if (enemy.IsParryGuageFull)
                {
                    ExecuteKillMove(enemy);
                }
                else
                {
                    // �и� �������� ���� �� ���� ���� ����� ó��
                    anim.SetTrigger("OnCloseAttackCombo");
                }
            }
        }
        else
        {
            // ����ĳ��Ʈ�� ������ ���� �ʾ��� ����� ó��
            anim.SetTrigger("OnCloseAttackCombo");
        }

        attackCount++;
    }

    void ExecuteKillMove(EnemyHealth enemy)
    {
        enemy.DisableBodyCollider();

        // �÷��̾��� ���� ��ġ�� ȸ���� �����ɴϴ�.
        Vector3 playerPosition = transform.position;
        Quaternion playerRotation = transform.rotation;

        enemy.transform.LookAt(playerPosition);

        // �÷��̾��� ���� ���� (z�� ����)���� 2.3 ���� ������ ��ġ�� ���� �̵�
        Vector3 forwardDirection = playerRotation * Vector3.forward; // �÷��̾ �ٶ󺸴� ����
        Vector3 enemyFinalPosition = playerPosition + forwardDirection * 2.3f; // �÷��̾� ���� 2.3 ���� ������ ��

        // ���� ��ġ�� ���� ��ġ�� ����
        enemy.transform.position = enemyFinalPosition;

        anim.SetTrigger("KillMove1");
        enemy.anim.SetTrigger("KillMove1");
    }

    public void ResetAttackCount()
    {
        attackCount = 0; // ���� Ƚ���� �ʱ�ȭ
    }

    public void ResetTrigger()
    {
        anim.ResetTrigger("OnCloseAttackCombo");
    }

    // ���� ������ �ű�� �Լ�
    public void DrawSword()
    {
        weapon.transform.localPosition = new Vector3(0.0094f,-0.0374f,0.1053f); // �ʿ��ϴٸ� ��ġ�� ȸ���� �����ϼ���
        weapon.transform.localRotation = Quaternion.Euler(-8.931f,262.3f,71.6f);
    }

    // ���� Į������ �ű�� �Լ�
    public void SheathSword()
    {
        weapon.transform.localPosition = new Vector3(-0.322f,-0.198f,0.029f); // �ʿ��ϴٸ� ��ġ�� ȸ���� �����ϼ���
        weapon.transform.localRotation = Quaternion.Euler(37.943f,-287.357f,-281.028f);
    }
}