using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int maxItems = 20;
    public ItemInstance[] items;

    private void Start()
    {
        items = new ItemInstance[maxItems];
    }

    public bool AddItem(ItemInstance newItem)
    {
        //adds to empty slot when it finds one
        for(int i = 0; i<items.Length;i++)
        {
            if (items[i]==null)
            {
                items[i] = newItem;
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
}
