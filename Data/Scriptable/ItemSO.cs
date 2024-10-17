using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item")]
public class ItemSO : ScriptableObject
{
    public string  itemName;
    public int itemId; // �������� ��Ÿ���� ��ȣ
    public Sprite sprite; // ������ �̹���
    public string description; // ������ ����
    public int purchasePrice;
    public int sellPrice;
    public bool stackable; // �ߺ� ��� ����
    public enum ItemType { Consumable, Equipment }
    public ItemType itemType;
    public int slotIndex;
}

[CreateAssetMenu(fileName = "New Equipment", menuName = "Item/Equipment")]
public class EquipmentSO : ItemSO
{
    public int statusAmount; // ���ǰ�� ���ݷ�/����
    public enum EquipType { Weapon, Armor } // ��� Ÿ��
    public EquipType equipType;
}

[CreateAssetMenu(fileName = "New Consumable", menuName = "Item/Consumable")]
public abstract class ConsumableSO : ItemSO
{
    public abstract void Use(PlayerState playerState);
}

[CreateAssetMenu(fileName = "New Potion", menuName = "Item/Potion")]
public class PotionSO : ConsumableSO
{
    public int recoveryAmount; // ������ ȸ����
    public enum PotionType { Health, Mana, Stamina } // ���� Ÿ��
    public PotionType potionType;
    public override void Use(PlayerState playerState)
    {
        playerState.playerStat.Heal(recoveryAmount);
        Debug.Log($"{description}��(��) ����߽��ϴ�.");
    }
}

[CreateAssetMenu(fileName = "New Miscellaneous", menuName = "Item/Miscellaneous")]
public class MiscellaneousSO : ItemSO
{
    // ���⿡ MiscellaneousSO�� Ưȭ�� �Ӽ��� �߰��� �� �ֽ��ϴ�.
}