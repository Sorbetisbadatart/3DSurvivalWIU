using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    private InventoryManager manager;

    public void OnDrop(PointerEventData eventData)
    {
        if(transform.childCount == 0)
        {
            InventoryItem item = eventData.pointerDrag.GetComponent<InventoryItem>();
            item.parentAfterDrag = transform;
        }
        //for adding item to another item
        else if(transform.GetChild(0).GetComponent<InventoryItem>().itemInstace.name == eventData.pointerDrag.GetComponent<InventoryItem>().itemInstace.name)
        {
            InventoryItem child = transform.GetChild(0).GetComponent<InventoryItem>();
            InventoryItem heldItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            //if its they can fit into one stack
            if (child.itemInstace.itemCount + heldItem.itemInstace.itemCount <= child.itemInstace.maxStack)
            {
                //adds to item count
                child.itemInstace.itemCount += heldItem.itemInstace.itemCount;

                //delete other item
                Destroy(eventData.pointerDrag);
                CallUpdate();
                child.UpdateCount();
            }
            else
            {
                int difference = (child.itemInstace.itemCount + heldItem.itemInstace.itemCount) - child.itemInstace.maxStack;
                child.itemInstace.itemCount = child.itemInstace.maxStack;
                heldItem.itemInstace.itemCount = difference;
                child.UpdateCount();
                heldItem.UpdateCount();
            }
        }
        //for making items change places
        else
        {
            InventoryItem tempChild = transform.GetChild(0).GetComponent<InventoryItem>();
            Transform tempSlot = eventData.pointerDrag.GetComponent<InventoryItem>().parentAfterDrag;

            InventoryItem item = eventData.pointerDrag.GetComponent<InventoryItem>();
            item.parentAfterDrag = transform;

            tempChild.parentAfterDrag = tempSlot;
            tempChild.UpdateLocation();
        }
    }

    public void SetManager(InventoryManager newManager)
    {
        manager = newManager;
    }

    public InventoryManager GetManager()
    {
        return manager;
    }

    public void CallUpdate()
    {
        manager.UpdateSlot();
    }
}
