using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SkillManager : MonoBehaviour
{
    public List<AudioClip> skillSounds;

    private Animator anim;
    private float radius = 5f;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void ActivateSkill(SkillSO skill)
    {
        Vector3 pos = transform.position;
        ParticleManager.instance.PlayParticle("JCE1",transform.position);
        ParticleManager.instance.PlayParticle("JCE2", transform.position);
        anim.Play("JCE1");
        StartCoroutine(WaitAndMoveBack(pos));
    }

    //public CameraController cam;
    IEnumerator WaitAndMoveBack(Vector3 originalPosition)
    {
        //cam.target = null;
        yield return new WaitForSeconds(2); // 첫 번째 대기
        VolumesManagers.Instance.ActiveSkillEffect();

        transform.position = new Vector3(10000, 10, 10000);

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
        foreach (var hitCollider in hitColliders)
        {
            EnemyHealth enemy = hitCollider.GetComponent<EnemyHealth>();
            if(enemy != null )
            {
                //enemy.TakeDamage(100);
            }
        }

        yield return new WaitForSeconds(2); // 두 번째 대기

        anim.Play("JCE2");
        transform.position = originalPosition;
        //cam.target = transform;

        yield return new WaitForSeconds(2);
        ParticleManager.instance.PlayParticle("JCE_Last", transform.position);
        VolumesManagers.Instance.DeactiveEffect();
    }
}
