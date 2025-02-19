using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    private Image image;

    public Transform parentAfterDrag;

    public ItemInstance itemInstace;

    [SerializeField] private TMP_Text countText;

    private void Start()
    {
        image = gameObject.GetComponent<Image>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        countText.text = itemInstace.itemCount.ToString();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
        parentAfterDrag.GetComponent<InventorySlot>().CallUpdate();
        countText.text = itemInstace.itemCount.ToString();
    }

    public void UpdateLocation()
    {
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
        parentAfterDrag.GetComponent<InventorySlot>().CallUpdate();
        countText.text = itemInstace.itemCount.ToString();
    }

    public void ObtainItem(ItemInstance newItem, int amt)
    {
        if(newItem != null)
        {
            itemInstace = newItem;
            itemInstace.itemCount = amt;

            if (image == null)
            {
                image = gameObject.GetComponent<Image>();
            }

            image.sprite = itemInstace.icon;
            countText.text = itemInstace.itemCount.ToString();
        }
    }

    public ItemInstance GetItem()
    {
        return itemInstace;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //if clicked split stack by half
        if(itemInstace.itemCount >= 2)
        {
            if(parentAfterDrag == null) return;
            //Debug.Log("PARENT!: " + parentAfterDrag.gameObject.name);
            itemInstace.itemCount -= 1;
            parentAfterDrag.gameObject.GetComponent<InventorySlot>().GetManager().AddItem(itemInstace, 1);
            countText.text = itemInstace.itemCount.ToString();
        }
    }

    public void UpdateCount()
    {
        countText.text = itemInstace.itemCount.ToString();
    }
}
