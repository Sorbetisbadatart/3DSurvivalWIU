using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, Iinteractable
{
    public Transform door; 
    public float Rotation;
    public float startRotation;
    public float endRotation;
    public float speed; 
    public bool isOpen = false;

    
    public void Interact()
    {
        Debug.Log("door");
      if (!isOpen)
            StartCoroutine(OpenDoor());
        else
            StartCoroutine(CloseDoor());
        isOpen = !isOpen;
    }
    public IEnumerator OpenDoor() 
    {
        isOpen = !isOpen;
        while (door.transform.rotation.y < endRotation) 
        {
            door.rotation = Quaternion.Euler(0, Rotation, 0);
            Rotation += speed * Time.deltaTime;
            
            yield return null; 
        }
    }
    public IEnumerator CloseDoor()
    {
        isOpen = !isOpen;
        while (door.transform.rotation.y > startRotation)
        {
            door.rotation = Quaternion.Euler(0, Rotation, 0);
            Rotation += speed * Time.deltaTime;

            yield return null;
        }
    }
}
