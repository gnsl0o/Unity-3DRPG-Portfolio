using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Stat
{
    public Action<float> OnStatChanged;

    private float baseValue;
    public float BaseValue
    {
        get { return baseValue;  }
        private set { baseValue = value; }
    }

    private float maxValue;
    public float MaxValue
    {
        get { return maxValue;  }
        set
        {
            maxValue = value;
            if (currentValue > maxValue)
            {
                currentValue = maxValue;
                OnStatChanged?.Invoke(currentValue);
            }
        }
    }

    private float currentValue;
    public float CurrentValue
    {
        get { return currentValue; }
        set
        {
            currentValue = Mathf.Clamp(value, 0, MaxValue);
            OnStatChanged?.Invoke(currentValue);
        }
    }

    private List<float> modifiers = new List<float>();

    public Stat(float baseValue, float maxValue)
    {
        BaseValue = baseValue;
        MaxValue = maxValue;
        CurrentValue = BaseValue;
    }

    public void AddModifier(float modifier)
    {
        modifiers.Add(modifier);
        CalculateFinalValue();
    }

    public void IncreaseMaxValue(float addtionalValue)
    {
        MaxValue += addtionalValue;
    }

    private void CalculateFinalValue()
    {
        CurrentValue = BaseValue;
        foreach (float modifier in modifiers)
        {
            CurrentValue += modifier;
        }

        CurrentValue = Mathf.Clamp(CurrentValue, 0, MaxValue);
        OnStatChanged?.Invoke(CurrentValue);
    }
}

public class CharacterStats
{
    public Stat Health;
    public Stat Mana;
    public Stat Stamina;
    public Stat Strength;
    public Stat Defense;

    public CharacterStats()
    {
        Health = new Stat(100, 100);
        Mana = new Stat(50, 50);
        Stamina = new Stat(100, 100);
        Strength = new Stat(10, 999);
        Defense = new Stat(0, 999);
    }
}
