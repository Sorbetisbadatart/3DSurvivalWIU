using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private int hotbarSize;

    [SerializeField] private GameObject inventorySlotPrefab;
    [SerializeField] private GameObject inventoryItemPrefab;

    [SerializeField] private GameObject hotbarUI;
    [SerializeField] private GameObject inventoryUI;

    private GameObject[] inventorySlots;

    [SerializeField] private GameObject InventoryPage;

    private void Awake()
    {
        inventorySlots = new GameObject[inventory.maxItems];

        for (int ii = 0; ii < hotbarSize; ii++)
        {
            inventorySlots[ii] = Instantiate(inventorySlotPrefab, hotbarUI.transform);
            inventorySlots[ii].GetComponent<InventorySlot>().SetManager(this);
        }

        //create inventory slots depending on amount of inventory size
        for (int i = hotbarSize; i < inventory.maxItems; i++)
        {
            inventorySlots[i] = Instantiate(inventorySlotPrefab, inventoryUI.transform);
            inventorySlots[i].GetComponent<InventorySlot>().SetManager(this);
        }

        //create items if there are items in the inventory
        for (int i = 0; i < inventory.maxItems; i++)
        {
            if (i < inventory.items.Length && inventory.items[i] != null)
            {
                GameObject temp = Instantiate(inventoryItemPrefab, inventorySlots[i].transform);
                temp.GetComponent<InventoryItem>().ObtainItem(inventory.items[i], 1);
            }
        }

        UpdateInventory();

        //give itself to inventory
        inventory.SetManager(this);

        //turn on canvas
        gameObject.GetComponent<Canvas>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            InventoryPage.SetActive(!InventoryPage.activeInHierarchy);
            ChangeMouseLock();
            AudioManager.Instance.PlaySFX("OpenInventory");
        }

        
        
    }
    public void ChangeMouseLock()
    {
        if (InventoryPage.activeInHierarchy)
        { 
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }



    //called when an item switches slot, to update array
    //create items in inventory if there are items in the ui but not in the inventory
    public void UpdateSlot()
    {
        for(int i = 0;i<inventory.maxItems;i++)
        {
            //if inventory slot is not null update inventory
            if (inventorySlots[i].transform.childCount > 0)
            {
                inventory.items[i] = inventorySlots[i].transform.GetChild(0).GetComponent<InventoryItem>().GetItem();
            }
            else
            {
                inventory.items[i] = null;
            }
        }
    }

    //opposite of update slot where it will create items if there are items in the inventory but not in the slots
    public void UpdateInventory()
    {
        //create items if there are items in the inventory
        for (int i = 0; i < inventory.maxItems; i++)
        {
            //add item
            if (i < inventory.items.Length && inventory.items[i] != null && inventory.items[i].itemType != null && inventorySlots[i].transform.childCount == 0)
            {
                GameObject temp = Instantiate(inventoryItemPrefab, inventorySlots[i].transform);
                temp.GetComponent<InventoryItem>().ObtainItem(inventory.items[i], inventory.items[i].itemCount);
            }
        }
    }

    //Updates ui from inventory
    public void UpdateInventoryUI()
    {
        //create items if there are items in the inventory
        for (int i = 0; i < inventory.maxItems; i++)
        {
            if (inventorySlots[i].transform.childCount > 0)
            {
                if (inventory.items[i] == null || inventory.items[i].itemType == null)
                {

                    Destroy(inventorySlots[i].transform.GetChild(0).gameObject);

                }
            }
        }
    }

    public void AddItem(ItemInstance item, int amt)
    {
        //look through inventory for empty slot
        for (int i = 0; i < inventory.maxItems; i++)
        {
            //Debug.Log("LOOP " + i);
            //Debug.Log("Inventory: " + inventory.items[i]);
            //Debug.Log("Inventory: " + inventory.items[i].itemType);
            //Strange bug here, sometimes wants items[i].itemType but sometimes dosent.
            if (inventory.items[i] == null || inventory.items[i].itemType == null)
            {
                //Debug.Log("INSTANTIATED");
                ItemInstance newItem = new ItemInstance(item.itemType);
                GameObject temp = Instantiate(inventoryItemPrefab, inventorySlots[i].transform);
                temp.GetComponent<InventoryItem>().ObtainItem(newItem, amt);
                break;
            }
        }
        UpdateSlot();
    }

    public void UpdateAllCount()
    {
        for(int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].transform.childCount > 0)
            {
                inventorySlots[i].transform.GetChild(0).GetComponent<InventoryItem>().UpdateCount();
            }
        }
    }
}
