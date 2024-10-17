using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemUsageHandler : MonoBehaviour
{

    public void UseItem(ItemSO item)
    {
        if (item is PotionSO potion)
        {
            switch (potion.potionType)
            {
                case PotionSO.PotionType.Health:
                    RecoverHealth(potion.recoveryAmount);
                    break;
                case PotionSO.PotionType.Mana:
                    RecoverMana(potion.recoveryAmount);
                    break;
                case PotionSO.PotionType.Stamina:
                    RecoverStamina(potion.recoveryAmount);
                    break;
            }
        }
        // �ٸ� ������ Ÿ�Կ� ���� ó���� ���⿡ �߰��� �� �ֽ��ϴ�.
    }
    private void RecoverHealth(int amount)
    {
        Player.Instance.HealHP(amount);
        Debug.Log("ȸ��");
        // ü�� ȸ�� ���� ó���� ���⿡ �߰��� �� �ֽ��ϴ�.
    }

    private void RecoverMana(int amount)
    {
        Player.Instance.HealMP(amount);
        // ���� ȸ�� ���� ó���� ���⿡ �߰��� �� �ֽ��ϴ�.
    }

    private void RecoverStamina(int amount)
    {
        Player.Instance.HealSTA(amount);
        // ���¹̳� ȸ�� ���� ó���� ���⿡ �߰��� �� �ֽ��ϴ�.
    }
}
