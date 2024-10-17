using UnityEngine;
using TMPro;

public class StatusUIManager : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI manaText;
    public TextMeshProUGUI staminaText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI defenceText;

    // ü�� ������Ʈ �Լ�
    public void UpdateHealthText(float health)
    {
        healthText.text = "ü�� : " + health.ToString();
    }

    // ���� ������Ʈ �Լ�
    public void UpdateManaText(float mana)
    {
        manaText.text = "���� : " + mana.ToString();
    }

    // ���¹̳� ������Ʈ �Լ�
    public void UpdateStaminaText(float stamina)
    {
        staminaText.text = "���¹̳� : " + stamina.ToString();
    }

    // ���ݷ� ������Ʈ �Լ�
    public void UpdateAttackText(float attack)
    {
        attackText.text = "���ݷ� : " + attack.ToString();
    }

    // ���� ������Ʈ �Լ�
    public void UpdateDefenceText(float defence)
    {
        defenceText.text = "���� : " + defence.ToString();
    }
}
