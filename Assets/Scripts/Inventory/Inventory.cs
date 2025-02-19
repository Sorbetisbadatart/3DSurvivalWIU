using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int maxItems = 28;
    public ItemInstance[] items;

    public InventoryManager manager;

    private void Start()
    {
        items = new ItemInstance[maxItems];
    }

    public bool AddItem(ItemInstance newItem, int amt)
    {
        //adds to empty slot when it finds one
        for(int i = 0; i<items.Length;i++)
        {
            if (items[i]  == null || items[i].itemType == null)
            {
                items[i] = newItem;
                items[i].itemCount = amt;
                manager.UpdateInventory();
                return true;
            }
        }

        //if(items.Length < maxItems)
        //{
        //    items.Add(newItem);
        //    return true;
        //}

        //if no space return false
        return false;
    }

    public void RemoveItem(ItemInstance itemToRemove)
    {

    }

    public ItemInstance GetItem(int num)
    {
        return items[num];
    }

    public void SetManager(InventoryManager newManager)
    {
        manager = newManager;
    }
}
