using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FoodData", menuName = "Food Data", order =2)]
public class FoodData : ItemData
{
    public float hungerRestore;
    public float thirstRestore;

    public override float Consume()
    {
        return hungerRestore;
    }

    public override float Drink()
    {
        return thirstRestore;
    }
}
