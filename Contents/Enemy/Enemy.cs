using System.Collections;
using System.Collections.Generic;

public class Enemy
{
    public string name;
    public int experienceValue;
    public ItemSO[] dropedItems;

    public Enemy(string name, int experienceValue, ItemSO[] dropedItems)
    {
        this.name = name;
        this.experienceValue = experienceValue;
        this.dropedItems = dropedItems;
    }
}
