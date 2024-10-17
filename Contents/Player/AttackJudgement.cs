using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackJudgement : MonoBehaviour
{
    // 데미지 이벤트 정의
    public delegate void HitEventHandler(int hitObject);
    public static event HitEventHandler OnHit;

    private int damageAmount = 2;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
            enemyHealth.TakeDamage(damageAmount);

            // 파티클 매니저를 통해 파티클 재생
            if(ParticleManager.instance != null)
            {
                ParticleManager.instance.PlayParticle("EnemyAttack", other.transform.position);
            }
        }
    }
}
