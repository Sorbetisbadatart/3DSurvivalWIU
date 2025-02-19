using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resoruce : MonoBehaviour
{
    public ItemData drop;

    [SerializeField] private int dropAmt = 1;

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
        if(collision.gameObject.tag == "Player")
        {
            ItemInstance newDrop = new ItemInstance(drop);
            collision.gameObject.GetComponent<Inventory>().AddItem(newDrop, dropAmt);
            Destroy(gameObject);
        }
    }
}
