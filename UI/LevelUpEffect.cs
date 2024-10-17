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
            // UI 프리팹 인스턴스화
            levelUpUiPrefab = Instantiate(Resources.Load<GameObject>("LevelUpPrefab"));

            // CanvasGroup을 사용해 페이드 효과 적용
            canvasGroup = levelUpUiPrefab.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = levelUpUiPrefab.AddComponent<CanvasGroup>();
            }

            TextMeshProUGUI levelUpText = levelUpUiPrefab.GetComponentInChildren<TextMeshProUGUI>();
            if (levelUpText != null)
            {
                levelUpText.text = "레벨 업! 현재 레벨: " + newLevel;
            }

            // 처음 나타날 때 페이드 인
            canvasGroup.alpha = 0; // 처음에는 투명하게 설정
            levelUpUiPrefab.SetActive(true);
            canvasGroup.DOFade(1, fadeDuration); // 페이드 인

            // 일정 시간 후 페이드 아웃 시작
            Invoke("FadeOutLevelUpPrefab", displayDuration);
        }
    }

    private void FadeOutLevelUpPrefab()
    {
        if (canvasGroup != null)
        {
            // 페이드 아웃 후 UI 파괴
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
