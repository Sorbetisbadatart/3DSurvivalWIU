using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class PreviewObject : MonoBehaviour
{
  


    public List<Collider> _collider = new List<Collider>();
    public Buildings sort;
    public Material green;
    public Material red;
    public bool canBuild = false;
   
    public bool second;
    public PreviewObject ChildCollider;
    public Transform MeshRenderer;

    private void Update()
    {
        if (!second)
            CanBePlaced();
    }

    


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
            _collider.Add(other);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 3)
            _collider.Remove(other);
    }

    public void CanBePlaced()
    {

        if (sort == Buildings.foundation)
           { //if it isnt blocked by anything, it is buildable
            if (_collider.Count == 0)
            {
                canBuild = true;
            }
            else
            {
                canBuild = false;
            }
        } 
        else
        {
            if ( ChildCollider._collider.Count > 0)
            {
                canBuild = true;
            }
            else
            {               
                canBuild = false;
            }
        }

        if (canBuild)
        {         
            foreach (Transform child in this.transform)
                    child.GetComponent<Renderer>().material = green;           
        }
        else
        {          
            foreach (Transform child in this.transform)
                child.GetComponent<Renderer>().material = red;          
        }
    }
}

public enum Buildings
{
    none = 0,
    foundation = 1,
    floor = 2,
    wall = 3,


}
