using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CookingRecipie", menuName = "Cooking Recipie Data", order = 4)]
public class CookingRecipie : ScriptableObject
{
    public FoodData item1;
    public int item1Amt;
    public FoodData item2;
    public int item2Amt;

    public FoodData result;
    public int resultAmt;
}
