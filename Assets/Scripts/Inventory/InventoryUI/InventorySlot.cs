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
            if (child.itemInstace.itemCount + eventData.pointerDrag.GetComponent<InventoryItem>().itemInstace.itemCount <= child.itemInstace.maxStack)
            {
                //adds to item count
                child.itemInstace.itemCount += eventData.pointerDrag.GetComponent<InventoryItem>().itemInstace.itemCount;

                //delete other item
                Destroy(eventData.pointerDrag);
                CallUpdate();
                child.UpdateCount();
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
