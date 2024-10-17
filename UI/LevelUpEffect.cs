using BehaviorDesigner.Runtime.Tasks.Unity.SharedVariables;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using TMPro;

public class LevelUpEffect : MonoBehaviour
{
    public ParticleSystem levelUpParticle;
    public AudioSource levelUpSound;
    public float displayDuration = 2f;
    public float fadeDuration = 0.5f;

    private GameObject levelUpUiPrefab;
    private CanvasGroup canvasGroup;

    void Start()
    {
        GameEventsManager.instance.playerEvents.onPlayerLevelChange += OnPlayerLevelUp;
    }

    private void OnPlayerLevelUp(int newLevel)
    {
        if (levelUpUiPrefab == null)
        {
            // UI ������ �ν��Ͻ�ȭ
            levelUpUiPrefab = Instantiate(Resources.Load<GameObject>("LevelUpPrefab"));

            // CanvasGroup�� ����� ���̵� ȿ�� ����
            canvasGroup = levelUpUiPrefab.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = levelUpUiPrefab.AddComponent<CanvasGroup>();
            }

            TextMeshProUGUI levelUpText = levelUpUiPrefab.GetComponentInChildren<TextMeshProUGUI>();
            if (levelUpText != null)
            {
                levelUpText.text = "���� ��! ���� ����: " + newLevel;
            }

            // ó�� ��Ÿ�� �� ���̵� ��
            canvasGroup.alpha = 0; // ó������ �����ϰ� ����
            levelUpUiPrefab.SetActive(true);
            canvasGroup.DOFade(1, fadeDuration); // ���̵� ��

            // ���� �ð� �� ���̵� �ƿ� ����
            Invoke("FadeOutLevelUpPrefab", displayDuration);
        }
    }

    private void FadeOutLevelUpPrefab()
    {
        if (canvasGroup != null)
        {
            // ���̵� �ƿ� �� UI �ı�
            canvasGroup.DOFade(0, fadeDuration).OnComplete(() =>
            {
                Destroy(levelUpUiPrefab);
            });
        }
    }

    private void OnDestroy()
    {
        GameEventsManager.instance.playerEvents.onPlayerLevelChange -= OnPlayerLevelUp;
    }
}
