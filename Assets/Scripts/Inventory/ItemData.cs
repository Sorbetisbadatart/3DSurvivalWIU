using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData",menuName ="Item Data",order = 1)]
public class ItemData : ScriptableObject
{
    public enum ItemStatus
    {
        Resource,
        Food,
        Tool
    }


    public string itemName;
    public Sprite icon;
    public int itemCount;
    public int maxStack;
    public ItemStatus itemStatus;
    [TextArea] public string description;

    public virtual float Consume()
    {
        return 0;
    }

    public virtual float Drink()
    {
        return 0;
    }

    public virtual FoodData GetFoodData()
    {
        return null;
    }
}
