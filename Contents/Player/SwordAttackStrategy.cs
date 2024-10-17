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

    private bool canInput = true; // 플레이어의 입력을 받을 수 있는지 여부

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

    // 애니메이션 이벤트로 호출될 메서드
    public void EnableInput()
    {
        canInput = true;
    }

    // 애니메이션 이벤트로 호출될 메서드
    public void DisableInput()
    {
        canInput = false;
    }

    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        // 입력을 받을 수 있는 상태이고, 플레이어가 공격 버튼을 눌렀을 경우
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
        // 레이캐스트의 시작점을 높이기 위해 y 축 값을 조정
        Vector3 startPosition = transform.position + new Vector3(0, 1.0f, 0); // 예: 무게중심보다 1.0 유닛 높게 설정
        Vector3 forward = transform.forward;

        if (Physics.Raycast(startPosition, forward, out hit, attackRange, 1 << 6))
        {
            EnemyHealth enemy = hit.collider.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                // 레이캐스트를 맞춘 적이 패링 게이지가 가득 차있는지 확인
                if (enemy.IsParryGuageFull)
                {
                    ExecuteKillMove(enemy);
                }
                else
                {
                    // 패링 게이지가 가득 차 있지 않을 경우의 처리
                    anim.SetTrigger("OnCloseAttackCombo");
                }
            }
        }
        else
        {
            // 레이캐스트가 적에게 맞지 않았을 경우의 처리
            anim.SetTrigger("OnCloseAttackCombo");
        }

        attackCount++;
    }

    void ExecuteKillMove(EnemyHealth enemy)
    {
        enemy.DisableBodyCollider();

        // 플레이어의 현재 위치와 회전을 가져옵니다.
        Vector3 playerPosition = transform.position;
        Quaternion playerRotation = transform.rotation;

        enemy.transform.LookAt(playerPosition);

        // 플레이어의 전방 방향 (z축 방향)으로 2.3 유닛 떨어진 위치로 적을 이동
        Vector3 forwardDirection = playerRotation * Vector3.forward; // 플레이어가 바라보는 방향
        Vector3 enemyFinalPosition = playerPosition + forwardDirection * 2.3f; // 플레이어 전방 2.3 유닛 떨어진 곳

        // 적의 위치를 최종 위치로 설정
        enemy.transform.position = enemyFinalPosition;

        anim.SetTrigger("KillMove1");
        enemy.anim.SetTrigger("KillMove1");
    }

    public void ResetAttackCount()
    {
        attackCount = 0; // 공격 횟수를 초기화
    }

    public void ResetTrigger()
    {
        anim.ResetTrigger("OnCloseAttackCombo");
    }

    // 검을 손으로 옮기는 함수
    public void DrawSword()
    {
        weapon.transform.localPosition = new Vector3(0.0094f,-0.0374f,0.1053f); // 필요하다면 위치와 회전을 조정하세요
        weapon.transform.localRotation = Quaternion.Euler(-8.931f,262.3f,71.6f);
    }

    // 검을 칼집으로 옮기는 함수
    public void SheathSword()
    {
        weapon.transform.localPosition = new Vector3(-0.322f,-0.198f,0.029f); // 필요하다면 위치와 회전을 조정하세요
        weapon.transform.localRotation = Quaternion.Euler(37.943f,-287.357f,-281.028f);
    }
}