using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int playerLevel = 1;
    private int playerExperience = 0;
    private int experienceToNextLevel = 100;

    void Start()
    {
        GameEventsManager.instance.enemyEvents.onEnemyDie += OnEnemyDie;
        GameEventsManager.instance.playerEvents.onExperienceGained += OnExperienceGained;
    }

    private void OnEnemyDie(Enemy enemy)
    {
        GameEventsManager.instance.playerEvents.ExperienceGained(enemy.experienceValue);
    }

    private void OnExperienceGained(int experienceGained)
    {
        playerExperience += experienceGained;

        if (playerExperience >= experienceToNextLevel)
        {
            LevelUp();
        }

        Debug.Log("경험치 획득 + 현재 경험치: " + playerExperience);
    }

    private void LevelUp()
    {
        playerLevel++;
        playerExperience = 0;
        experienceToNextLevel += 100;

        GameEventsManager.instance.playerEvents.PlayerLevelUp(playerLevel);
    }
}
