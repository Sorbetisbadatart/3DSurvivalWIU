using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Cooking : MonoBehaviour
{
    [SerializeField] private CookingRecipie recipie;
    [SerializeField] private TMP_Text recipieText;

    private Inventory inventory;

    private bool canCook;

    [SerializeField] private CookingManger cookManager;

    private void Start()
    {
        inventory = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>().GetInventory();
        recipieText.text = recipie.item1.name + inventory.CheckItemCount(recipie.item1) + "/" +recipie.item1Amt;
        recipieText.text += "\n" + recipie.item2.name + inventory.CheckItemCount(recipie.item2) + "/" + recipie.item2Amt;
    }

    private void LateUpdate()
    {
        recipieText.text = recipie.item1.name + inventory.CheckItemCount(recipie.item1) + "/" + recipie.item1Amt;
        recipieText.text += "\n" + recipie.item2.name + inventory.CheckItemCount(recipie.item2) + "/" + recipie.item2Amt;

        if (inventory.CheckItemCount(recipie.item1) >= recipie.item1Amt && inventory.CheckItemCount(recipie.item2) >= recipie.item2Amt)
        {
            canCook = true;
        }
        else
        {
            canCook = false;
        }
    }

    //start cooking minigame if there are items to use to cook
    public void Cook()
    {
        if(canCook)
        {
            inventory.RemoveItem(recipie.item1, recipie.item1Amt);
            inventory.RemoveItem(recipie.item2, recipie.item2Amt);
            cookManager.StartCookingMinigame(recipie.result);
        }
    }
}
