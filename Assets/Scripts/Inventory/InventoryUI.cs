using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Inventory inventory;

    [SerializeField] private Button[] inventorySlots;

    private ItemInstance mouseInventory;
    

    // Start is called before the first frame update
    void Start()
    {
        UpdateInventory();
    }

    void UpdateInventory()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (i < inventory.items.Length)
            {
                //inventorySlots[i].gameObject.SetActive(true);
                //inventorySlots[i].gameObject.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.GetItem(i).itemType.icon;
            }
            else
            {
                //inventorySlots[i].gameObject.SetActive(false);
            }
        }
    }

    public void GrabInventorySlot()
    {
        if(mouseInventory == null)
        {
            //mouseInventory = 
        }
    }
}
