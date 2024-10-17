using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour, ISaveable
{
    public PlayerContext playerContext;

    public PlayerStat playerStat;
    public StatusUIManager statusUI;

    public Parry parry;

    private bool canBeHit = true;
    private bool isHit = false;

    public BoxCollider parryCollider;
    public CapsuleCollider hitBoxCollider;

    public Slider healthSlider;
    public Slider manaSlider;

    private void Start()
    {
        // PlayerStat이 null이면 새로 초기화
        if (playerStat == null)
        {
            playerStat = new PlayerStat();
        }

        // StatusUIManager 참조 설정 (null일 경우 FindObjectOfType로 찾기)
        if (statusUI == null)
        {
            statusUI = FindObjectOfType<StatusUIManager>();
        }

        // 게임 시작 시 현재 스탯을 UI에 반영
        ApplyCurrentStateToUI();

        // 슬라이더 초기값 설정
        healthSlider.value = playerStat.stats.Health.CurrentValue;
        manaSlider.value = playerStat.stats.Mana.CurrentValue;

        // 스탯이 변경될 때마다 UI 업데이트
        playerStat.stats.Health.OnStatChanged += UpdateHealthSlider;
        playerStat.stats.Health.OnStatChanged += statusUI.UpdateHealthText;

        playerStat.stats.Mana.OnStatChanged += UpdateManaSlider;
        playerStat.stats.Mana.OnStatChanged += statusUI.UpdateManaText;

        // 추가: 스태미나, 공격력, 방어력 스탯이 변경될 때 UI 업데이트 등록
        playerStat.stats.Stamina.OnStatChanged += statusUI.UpdateStaminaText;
        playerStat.stats.Strength.OnStatChanged += statusUI.UpdateAttackText;
        playerStat.stats.Defense.OnStatChanged += statusUI.UpdateDefenceText;
    }

    void Die()
    {
        playerContext.Input.DisableAllPlayerActions();
        playerContext.anim.SetTrigger("Die");
    }

    public void ActiveParryCollider()
    {
        parryCollider.enabled = true;
        hitBoxCollider.enabled = false;
    }

    public void DeActiveParryCollider()
    {
        parryCollider.enabled = false;
        hitBoxCollider.enabled = true;
    }

    public IEnumerator Invulnerability(float duration)
    {
        canBeHit = false;
        yield return new WaitForSeconds(duration);
        canBeHit = true;
    }
    private IEnumerator ResetHitState()
    {
        yield return new WaitForSeconds(1.0f); // 피격 상태 지속 시간 설정
        isHit = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (parryCollider.enabled && other.CompareTag("EnemyWeapon") && !isHit)
        {
            Debug.Log("패링 성공");
            // 파티클 매니저를 통해 파티클 재생
            if (ParticleManager.instance != null)
            {
                DeActiveParryCollider();
                StartCoroutine(Invulnerability(0.5f)); // 0.5초 동안 피격 불가
                playerContext.anim.SetTrigger("Player_Parry");
                other.GetComponentInParent<Animator>().SetTrigger("Enemy_Parried"); // 수정할 부분
                ParticleManager.instance.PlayParticle("Parry", other.transform.position);
                AudioManager.instance.PlaySFX("ParrySound");
            }
        }
        else if (hitBoxCollider.enabled && other.CompareTag("EnemyWeapon") && canBeHit)
        {
            // 피격 로직
            isHit = true;
            if (playerContext.actionStateMachine.CurrentState == playerContext.actionStateMachine.SheathedState)
                playerContext.anim.SetTrigger("PlayerHit1");
            else
            {
                playerContext.anim.SetTrigger("PlayerHit2");
            }

            playerStat.TakeDamage(10);
        }
    }

    public void SaveData(GameData data)
    {
        data.playerHealth = 1;
        data.playerPositionX = transform.position.x;
        data.playerPositionY = transform.position.y;
        data.playerPositionZ = transform.position.z;
    }

    public void LoadData(GameData data)
    {
        
    }

    private void UpdateHealthSlider(float newHealth)
    {
        healthSlider.value = newHealth;
    }

    private void UpdateManaSlider(float newMana)
    {
        manaSlider.value = newMana;
    }

    private void ApplyCurrentStateToUI()
    {
        statusUI.UpdateHealthText(playerStat.stats.Health.CurrentValue);
        statusUI.UpdateManaText(playerStat.stats.Mana.CurrentValue);
        statusUI.UpdateStaminaText(playerStat.stats.Stamina.CurrentValue);
        statusUI.UpdateAttackText(playerStat.stats.Strength.CurrentValue);
        statusUI.UpdateDefenceText(playerStat.stats.Defense.CurrentValue);
    }

    public void PlayerDeath()
    {
        playerContext.anim.SetTrigger("Die");
    }
}
