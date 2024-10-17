using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    private Enemy enemy;

    public int maxHealth = 10; // 적의 최대 체력
    private int currentHealth; // 현재 체력

    private float lerpSpeed = 5f; // Lerp 속도 조절
    private float targetHealth; // 목표 체력 값

    public Slider healthBar;
    public float duration = 3.0f; // HP 바의 지속시간
    private Coroutine healthBarCoroutine = null; // 현재 실행 중인 코루틴을 추적하기 위한 변수
    public Animator anim;

    public Collider bodyCollider;

    public bool IsParryGuageFull = true; // 임시

    public Transform marker;

    public int CurrentHealth // CurrentHealth 프로퍼티
    {
        get { return currentHealth; } // getter 메서드
    }

    void Start()
    {
        enemy = new Enemy("Robot", 50, null);
    }

    void OnEnable()
    {
        currentHealth = maxHealth; // 시작 시 현재 체력을 최대 체력으로 설정
        targetHealth = currentHealth;

        anim = GetComponent<Animator>();

        // HealthBarManager에서 HealthBar를 가져와서 할당
        healthBar = HealthBarManager.Instance.GetHealthBar();

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
        else
        {
            Debug.LogError("HealthBar가 할당되지 않았습니다.");
        }

        AttackJudgement.OnHit += TakeDamage;
    }

    // 피격 시 호출되는 메서드
    public void TakeDamage(int damageAmount)
    {
        anim.SetTrigger("EnemyHit");

        if (healthBarCoroutine != null)
        {
            StopCoroutine(healthBarCoroutine); // 진행 중인 코루틴이 있다면 중단
        }
        healthBarCoroutine = StartCoroutine(ActivateForSeconds(duration)); // 코루틴 재시작

        currentHealth -= damageAmount; // 현재 체력을 감소
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // 최소값 0, 최대값 최대 체력으로 제한

        targetHealth = currentHealth;

        if (currentHealth <= 0)
        {
            Die(enemy); // 체력이 0 이하일 경우 사망 처리
        }
    }

    // 사망 시 호출되는 메서드
    void Die(Enemy enemy)
    {
        // 적의 사망 처리 작성 (예: 폭발 효과, 사운드 재생 등)
        GameEventsManager.instance.enemyEvents.EnemyDie(enemy);
        HealthBarManager.Instance.ReturnHealthBar(healthBar); // 체력바 반환
        Destroy(gameObject); // 현재 게임 오브젝트 파괴
    }

    // HP 바를 업데이트하는 메서드
    void UpdateHealthBar()
    {
        healthBar.value = currentHealth;
    }

    IEnumerator ActivateForSeconds(float seconds)
    {
        healthBar.gameObject.SetActive(true); // 체력 바 활성화
        yield return new WaitForSeconds(seconds);
        healthBar.gameObject.SetActive(false); // 비활성화

        healthBarCoroutine = null; // 코루틴 참조 초기화
    }

    void ActivateHealthBar()
    {
        healthBar.gameObject.SetActive(true); // 체력 바 활성화
    }

    void DeActiveHealthBar()
    {
        healthBar.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (healthBar != null)
        {
            // 적의 머리 위에 체력바 위치 조정
            Vector3 healthBarPosition = transform.position + new Vector3(0, 1.5f, 0);
            healthBar.transform.position = healthBarPosition;

            // 필요하다면, 카메라를 바라보도록 조정
            healthBar.transform.LookAt(Camera.main.transform);
        }

        if (healthBar.value != targetHealth)
        {
            healthBar.value = Mathf.Lerp(healthBar.value, targetHealth, Time.deltaTime * lerpSpeed);
        }
    }

    public void EnableCollider()
    {
        bodyCollider.enabled = true;
    }
    public void DisableBodyCollider()
    {
        bodyCollider.enabled = false;
    }
}
