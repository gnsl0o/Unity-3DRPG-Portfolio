using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackJudgement : MonoBehaviour
{
    // ������ �̺�Ʈ ����
    public delegate void HitEventHandler(int hitObject);
    public static event HitEventHandler OnHit;

    private int damageAmount = 2;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
            enemyHealth.TakeDamage(damageAmount);

            // ��ƼŬ �Ŵ����� ���� ��ƼŬ ���
            if(ParticleManager.instance != null)
            {
                ParticleManager.instance.PlayParticle("EnemyAttack", other.transform.position);
            }
        }
    }
}
