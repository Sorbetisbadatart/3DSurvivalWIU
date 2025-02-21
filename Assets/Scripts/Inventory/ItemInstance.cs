using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemInstance
{
    public ItemData itemType;
    public String name;
    public Sprite icon;
    public int itemCount;
    public int maxStack;
    public string description;
    public int itemStatus;

    public ItemInstance(ItemData itemData)
    {
        itemType = itemData;
        name = itemData.name;
        icon = itemData.icon;
        itemCount = itemData.itemCount;
        maxStack = itemData.maxStack;
        description = itemData.description;
        itemStatus = (int)itemData.itemStatus;
    }
}
