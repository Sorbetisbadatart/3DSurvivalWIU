using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : MonoBehaviour
{
    [SerializeField] private CookingManger manager;

    private bool playerInside = false;
    private bool listOpen = false;

    private void Start()
    {
        if(manager == null)
        {
            manager = GameObject.Find("CookingCanvas").GetComponent<CookingManger>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            manager.SetCookingListActive(false);
            playerInside = false;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            if (playerInside)
            {
                if(listOpen)
                {
                    manager.SetCookingListActive(true);
                    listOpen = false;
                }
                else
                {
                    manager.SetCookingListActive(false);
                    listOpen = true;
                }
            }
        }
    }
}
