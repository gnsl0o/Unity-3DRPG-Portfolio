using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item")]
public class ItemSO : ScriptableObject
{
    public string  itemName;
    public int itemId; // 아이템을 나타내는 번호
    public Sprite sprite; // 아이템 이미지
    public string description; // 아이템 설명
    public int purchasePrice;
    public int sellPrice;
    public bool stackable; // 중복 허용 여부
    public enum ItemType { Consumable, Equipment }
    public ItemType itemType;
    public int slotIndex;
}

[CreateAssetMenu(fileName = "New Equipment", menuName = "Item/Equipment")]
public class EquipmentSO : ItemSO
{
    public int statusAmount; // 장비품의 공격력/방어력
    public enum EquipType { Weapon, Armor } // 장비 타입
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
    public int recoveryAmount; // 포션의 회복량
    public enum PotionType { Health, Mana, Stamina } // 포션 타입
    public PotionType potionType;
    public override void Use(PlayerState playerState)
    {
        playerState.playerStat.Heal(recoveryAmount);
        Debug.Log($"{description}을(를) 사용했습니다.");
    }
}

[CreateAssetMenu(fileName = "New Miscellaneous", menuName = "Item/Miscellaneous")]
public class MiscellaneousSO : ItemSO
{
    // 여기에 MiscellaneousSO에 특화된 속성을 추가할 수 있습니다.
}