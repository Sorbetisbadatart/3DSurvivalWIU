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
    }

    public void GetManager(InventoryManager newManager)
    {
        manager = newManager;
    }

    public void CallUpdate()
    {
        manager.UpdateSlot();
    }
}
