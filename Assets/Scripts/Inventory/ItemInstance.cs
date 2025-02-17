using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemInstance
{
    public ItemData itemType;
    public string description;

    public ItemInstance(ItemData itemData)
    {
        itemType = itemData;
        description = itemData.description;
    }
}
