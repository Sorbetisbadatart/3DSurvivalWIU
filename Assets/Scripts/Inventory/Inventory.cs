using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int maxItems = 28;
    public ItemInstance[] items;

    public InventoryManager manager;

    public ItemInstance equippedSlot;

    private ThirstNHunger thirstNHunger;

    private void Start()
    {
        items = new ItemInstance[maxItems];
        thirstNHunger = gameObject.GetComponent<ThirstNHunger>();
    }

    public bool AddItem(ItemInstance newItem, int amt)
    {
        for (int i = 0; i < items.Length; i++)
        {
            //check if there item is able to be stacked with another item
            if (items[i].itemType == newItem.itemType && (items[i].itemCount + amt) <= items[i].maxStack)
            {
                //if it fits into the stack
                items[i].itemCount += amt;
                manager.UpdateAllCount();
                return true;
            }
            else if (items[i].itemType == newItem.itemType && (items[i].itemCount + amt) > items[i].maxStack)
            {
                //make a new stack
                int overflow = (items[i].itemCount + amt) - items[i].maxStack;
                items[i].itemCount = items[i].maxStack;
                OverFlowAddItem(newItem, overflow);
            }
            else
            {
                //adds to empty slot when it finds one
                if (items[i] == null || items[i].itemType == null)
                {
                    items[i] = newItem;
                    items[i].itemCount = amt;
                    manager.UpdateInventory();
                    return true;
                }
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

    private void OverFlowAddItem(ItemInstance newItem, int amt)
    {
        for (int i = 0; i < items.Length; i++)
        {
            //adds to empty slot when it finds one
            if (items[i] == null || items[i].itemType == null)
            {
                items[i] = newItem;
                items[i].itemCount = amt;
                manager.UpdateInventory();
                break;
            }
        }
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

    private void Update()
    {
        //for use to equip item in a certain slot to use
        if (Input.GetKeyDown("1"))
        {
            equippedSlot = items[0];
        }
        else if(Input.GetKeyDown("2"))
        {
            equippedSlot = items[1];
        }
        else if (Input.GetKeyDown("3"))
        {
            equippedSlot = items[2];
        }
        else if (Input.GetKeyDown("4"))
        {
            equippedSlot = items[3];
        }
        else if (Input.GetKeyDown("5"))
        {
            equippedSlot = items[4];
        }
        else if (Input.GetKeyDown("6"))
        {
            equippedSlot = items[5];
        }
        else if (Input.GetKeyDown("7"))
        {
            equippedSlot = items[6];
        }

        if (Input.GetKeyDown("f"))
        {
            //use item
            Debug.Log(equippedSlot.itemType.Consume());
            thirstNHunger.GainHunger(equippedSlot.itemType.Consume());
        }
    }
}
