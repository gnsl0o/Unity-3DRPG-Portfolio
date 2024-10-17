using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "PlayerStat", menuName = "Stats/PlayerStat")]
public class PlayerStatSO : ScriptableObject
{
    public CharacterStats stats = new CharacterStats();

    public event Action OnPlayerDie;

    public void TakeDamage(float amount)
    {
        float actualAmount = amount - stats.Defense.CurrentValue;
        if (actualAmount < 0) actualAmount = 0;

        stats.Health.CurrentValue -= actualAmount;

        if (stats.Health.CurrentValue <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        stats.Health.CurrentValue += amount;
        if (stats.Health.CurrentValue > stats.Health.MaxValue)
        {
            stats.Health.CurrentValue = stats.Health.MaxValue;
        }
    }

    public void ApplyStrengthBuff(float buffAmount)
    {
        stats.Strength.AddModifier(buffAmount);
        Debug.Log("Player's strength buffed: " + stats.Strength.CurrentValue);
    }

    public void LevelUp()
    {
        stats.Health.IncreaseMaxValue(20);
        stats.Mana.IncreaseMaxValue(10);
    }

    private void Die()
    {
        OnPlayerDie?.Invoke(); // 플레이어 사망 이벤트 발생
    }
}
