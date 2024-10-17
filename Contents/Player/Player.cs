using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int maxHealth = 1000;
    public int currentHealth = 50;
    public int maxMana = 50;
    public int currentMana = 50;
    public int maxStamina = 100;
    public int currentStamina = 100;

    // HP ȸ��
    public void HealHP(int amount)
    {
        currentHealth += amount;
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        Debug.Log("ȸ�� �Ϸ� + " + currentHealth + " " + amount);
    }

    // MP ȸ��
    public void HealMP(int amount)
    {
        currentMana += amount;
        if(currentMana > maxMana)
        {
            currentMana = maxMana;
        }
    }

    // STA ȸ��
    public void HealSTA(int amount)
    {
        currentStamina += amount;
        if(currentStamina > maxStamina)
        {
            currentStamina = maxStamina;
        }
    }
}
