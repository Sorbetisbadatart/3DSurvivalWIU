using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Image image;

    public Transform parentAfterDrag;

    public ItemInstance itemInstace;

    [SerializeField] private TMP_Text countText;

    [SerializeField] private GameObject descriptionBox;

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
    public void OnPointerEnter(PointerEventData eventData)
    {
        //when player hovers over item display item details
        descriptionBox.SetActive(true);
        descriptionBox.transform.position = Input.mousePosition;
        descriptionBox.GetComponentInChildren<TMP_Text>().text = itemInstace.name + "\n" + itemInstace.description;
        descriptionBox.transform.SetParent(descriptionBox.transform.root);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        descriptionBox.transform.SetParent(transform.root);
        descriptionBox.SetActive(false);
    }

    public void UpdateCount()
    {
        countText.text = itemInstace.itemCount.ToString();
    }
}
