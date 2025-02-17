using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PreviewObject : MonoBehaviour
{
    public bool foundation;
    public List<Collider> _collider = new List<Collider>();
    public Material green;
    public Material red;
    public bool canBuild = false;

    private void Update()
    {
        CanBePlaced();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3 )
            if (!_collider.Contains(other))
            {
                _collider.Add(other);
               
                
                
            }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 3 )
            if (!_collider.Contains(other))
            {
                _collider.Remove(other);
                
               
            }
    }

    public void CanBePlaced()
    {
    Debug.Log(_collider.Count);
        Debug.Log(canBuild);
        //if it isnt blocked by anything, it is buildable
        if (_collider.Count == 0)
        {
            canBuild = true;
        }
        else
        {
            canBuild = false;

        }


        foreach (Transform child in this.transform)
        {
            if (canBuild)
            {
                child.GetComponent<Renderer>().material = green;
               
            }
            else
            {
                child.GetComponent<Renderer>().material = red;
                Debug.Log("Red");
                break;
            }
        }

       



    }
}
