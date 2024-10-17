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
        // 다른 아이템 타입에 대한 처리도 여기에 추가할 수 있습니다.
    }
    private void RecoverHealth(int amount)
    {
        Player.Instance.HealHP(amount);
        Debug.Log("회복");
        // 체력 회복 후의 처리를 여기에 추가할 수 있습니다.
    }

    private void RecoverMana(int amount)
    {
        Player.Instance.HealMP(amount);
        // 마나 회복 후의 처리를 여기에 추가할 수 있습니다.
    }

    private void RecoverStamina(int amount)
    {
        Player.Instance.HealSTA(amount);
        // 스태미나 회복 후의 처리를 여기에 추가할 수 있습니다.
    }
}
