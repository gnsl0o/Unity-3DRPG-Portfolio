using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat
{
    public CharacterStats stats = new CharacterStats();

    public void TakeDamage(float damage)
    {
        float actualDamage = damage - stats.Defense.CurrentValue;
        if (actualDamage < 0) actualDamage = 0;

        stats.Health.CurrentValue -= actualDamage;
    }

    public void Heal(float amount)
    {
        stats.Health.CurrentValue += amount;
        if (stats.Health.CurrentValue > stats.Health.BaseValue)
        {
            stats.Health.CurrentValue = stats.Health.BaseValue;
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
}
