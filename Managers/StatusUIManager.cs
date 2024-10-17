using UnityEngine;
using TMPro;

public class StatusUIManager : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI manaText;
    public TextMeshProUGUI staminaText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI defenceText;

    // 체력 업데이트 함수
    public void UpdateHealthText(float health)
    {
        healthText.text = "체력 : " + health.ToString();
    }

    // 마나 업데이트 함수
    public void UpdateManaText(float mana)
    {
        manaText.text = "마나 : " + mana.ToString();
    }

    // 스태미나 업데이트 함수
    public void UpdateStaminaText(float stamina)
    {
        staminaText.text = "스태미나 : " + stamina.ToString();
    }

    // 공격력 업데이트 함수
    public void UpdateAttackText(float attack)
    {
        attackText.text = "공격력 : " + attack.ToString();
    }

    // 방어력 업데이트 함수
    public void UpdateDefenceText(float defence)
    {
        defenceText.text = "방어력 : " + defence.ToString();
    }
}
