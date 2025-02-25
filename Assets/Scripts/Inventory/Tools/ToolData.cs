using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ToolData", menuName = "Tool Data", order = 3)]
public class ToolData : ItemData
{
    public int durability;

    public FoodData food;

    public override FoodData GetFoodData()
    {
        return food;
    }
}
