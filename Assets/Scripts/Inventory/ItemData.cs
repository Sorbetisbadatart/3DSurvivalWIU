using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData",menuName ="Item Data",order = 1)]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public int itemCount;
    public int maxStack;
    [TextArea] public string description;
}
