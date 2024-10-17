using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager instance { get; private set; }

    private Dictionary<string, List<ParticleSystem>> particlePools;
    private Dictionary<string, ParticleSystem> particlePrefabs;

    public int initialSize = 5;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }

        particlePools = new Dictionary<string, List<ParticleSystem>>();
        particlePrefabs = new Dictionary<string, ParticleSystem>();
    }

    public void PlayParticle(string particleName, Vector3 position)
    {
        if (!particlePools.ContainsKey(particleName))
        {
            // 첫 요청 시 리소스 폴더에서 파티클 로드 및 풀 생성
            ParticleSystem loadedParticle = Resources.Load<ParticleSystem>("Particles/" + particleName);
            
            if (loadedParticle != null)
            {
                particlePrefabs[particleName] = loadedParticle;
                List<ParticleSystem> pools = new List<ParticleSystem>();
                for (int i = 0; i < initialSize; i++)
                {
                    ParticleSystem newParticle = Instantiate(loadedParticle);
                    newParticle.gameObject.SetActive(false);
                    pools.Add(newParticle);
                }
                particlePools[particleName] = pools;
            }
            else
            {
                Debug.LogError("일치하는 파티클이 존재하지 않음 " + particleName);
                return;
            }
        }

        List<ParticleSystem> pool = particlePools[particleName];
        ParticleSystem particle = GetParticleFromPool(pool, particlePrefabs[particleName]);

        if (particle != null)
        {
            particle.transform.position = position;
            StartCoroutine(PlayAndDisable(particle));
        }
    }

    private ParticleSystem GetParticleFromPool(List<ParticleSystem> pool,  ParticleSystem prefab)
    {
        foreach (ParticleSystem particle in pool)
        {
            if (!particle.gameObject.activeInHierarchy)
            {
                return particle;
            }
        }

        ParticleSystem newParticle = Instantiate(prefab);
        newParticle.gameObject.SetActive(false);
        pool.Add(newParticle);
        return newParticle;
    }

    private IEnumerator PlayAndDisable(ParticleSystem particle)
    {
        particle.gameObject.SetActive(true);
        particle.Play();

        yield return new WaitForSeconds(particle.main.duration);

        particle.gameObject.SetActive(false);
        particle.transform.position = Vector3.zero; 
        particle.transform.rotation = Quaternion.identity;
    }
}
