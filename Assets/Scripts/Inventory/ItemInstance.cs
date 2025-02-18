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
    public string description;

    public ItemInstance(ItemData itemData)
    {
        itemType = itemData;
        name = itemData.name;
        icon = itemData.icon;
        description = itemData.description;
    }
}
