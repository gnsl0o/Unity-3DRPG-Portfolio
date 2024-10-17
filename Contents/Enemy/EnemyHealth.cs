using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    private Enemy enemy;

    public int maxHealth = 10; // ���� �ִ� ü��
    private int currentHealth; // ���� ü��

    private float lerpSpeed = 5f; // Lerp �ӵ� ����
    private float targetHealth; // ��ǥ ü�� ��

    public Slider healthBar;
    public float duration = 3.0f; // HP ���� ���ӽð�
    private Coroutine healthBarCoroutine = null; // ���� ���� ���� �ڷ�ƾ�� �����ϱ� ���� ����
    public Animator anim;

    public Collider bodyCollider;

    public bool IsParryGuageFull = true; // �ӽ�

    public Transform marker;

    public int CurrentHealth // CurrentHealth ������Ƽ
    {
        get { return currentHealth; } // getter �޼���
    }

    void Start()
    {
        enemy = new Enemy("Robot", 50, null);
    }

    void OnEnable()
    {
        currentHealth = maxHealth; // ���� �� ���� ü���� �ִ� ü������ ����
        targetHealth = currentHealth;

        anim = GetComponent<Animator>();

        // HealthBarManager���� HealthBar�� �����ͼ� �Ҵ�
        healthBar = HealthBarManager.Instance.GetHealthBar();

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
        else
        {
            Debug.LogError("HealthBar�� �Ҵ���� �ʾҽ��ϴ�.");
        }

        AttackJudgement.OnHit += TakeDamage;
    }

    // �ǰ� �� ȣ��Ǵ� �޼���
    public void TakeDamage(int damageAmount)
    {
        anim.SetTrigger("EnemyHit");

        if (healthBarCoroutine != null)
        {
            StopCoroutine(healthBarCoroutine); // ���� ���� �ڷ�ƾ�� �ִٸ� �ߴ�
        }
        healthBarCoroutine = StartCoroutine(ActivateForSeconds(duration)); // �ڷ�ƾ �����

        currentHealth -= damageAmount; // ���� ü���� ����
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // �ּҰ� 0, �ִ밪 �ִ� ü������ ����

        targetHealth = currentHealth;

        if (currentHealth <= 0)
        {
            Die(enemy); // ü���� 0 ������ ��� ��� ó��
        }
    }

    // ��� �� ȣ��Ǵ� �޼���
    void Die(Enemy enemy)
    {
        // ���� ��� ó�� �ۼ� (��: ���� ȿ��, ���� ��� ��)
        GameEventsManager.instance.enemyEvents.EnemyDie(enemy);
        HealthBarManager.Instance.ReturnHealthBar(healthBar); // ü�¹� ��ȯ
        Destroy(gameObject); // ���� ���� ������Ʈ �ı�
    }

    // HP �ٸ� ������Ʈ�ϴ� �޼���
    void UpdateHealthBar()
    {
        healthBar.value = currentHealth;
    }

    IEnumerator ActivateForSeconds(float seconds)
    {
        healthBar.gameObject.SetActive(true); // ü�� �� Ȱ��ȭ
        yield return new WaitForSeconds(seconds);
        healthBar.gameObject.SetActive(false); // ��Ȱ��ȭ

        healthBarCoroutine = null; // �ڷ�ƾ ���� �ʱ�ȭ
    }

    void ActivateHealthBar()
    {
        healthBar.gameObject.SetActive(true); // ü�� �� Ȱ��ȭ
    }

    void DeActiveHealthBar()
    {
        healthBar.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (healthBar != null)
        {
            // ���� �Ӹ� ���� ü�¹� ��ġ ����
            Vector3 healthBarPosition = transform.position + new Vector3(0, 1.5f, 0);
            healthBar.transform.position = healthBarPosition;

            // �ʿ��ϴٸ�, ī�޶� �ٶ󺸵��� ����
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
