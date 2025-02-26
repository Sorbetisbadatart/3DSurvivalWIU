using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Resoruce : MonoBehaviour
{
    public ItemData drop;

    [SerializeField] private int dropAmt = 1;

    [SerializeField] private float timeToObtain = 0f;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        //return if has time to collect
        if (timeToObtain > 0f) return;

        if(collision.gameObject.tag == "Player")
        {
            AudioManager.Instance.PlaySFX("Pickup");
            ItemInstance newDrop = new ItemInstance(drop);
            TextManager.TextInstance.CreateText(new Vector3(350, 800, 1), "Picked up " + newDrop.name, Color.white);

            collision.gameObject.GetComponent<Inventory>().AddItem(newDrop, dropAmt);
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //return if time to collect is 0 or less
        if (timeToObtain <= 0f) return;

        if(other.gameObject.tag == "Player")
        {
            Inventory playerInv = other.gameObject.GetComponent<Inventory>();

            //pick up after certain amount of time
            if(timer >= timeToObtain)
            {
                playerInv.manager.HandPercentage(0, false);
                ItemInstance newDrop = new ItemInstance(drop);
                playerInv.AddItem(newDrop, dropAmt);
                Destroy(gameObject);
            }
            else
            {
                timer += Time.deltaTime;

                playerInv.manager.HandPercentage((timer / timeToObtain), true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        timer = 0;
        other.gameObject.GetComponent<Inventory>().manager.HandPercentage(0, false);
    }
}
