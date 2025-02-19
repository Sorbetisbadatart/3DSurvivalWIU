using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour, Iinteractable
{
    public Transform door; 
    public float Rotation;
    public float startRotation;
    public float endRotation;
    public float OriginalRotation;
    public float speed; 
    public bool isOpen = false;

  
    public IEnumerator OpenDoor() 
    {
        
        while (Rotation < 90 )
        {
            door.localRotation = Quaternion.Euler(0, Rotation, 0);
            
            
            Rotation += speed * Time.deltaTime;
            
            yield return null; 
        }
        isOpen = !isOpen;
    }
    public IEnumerator CloseDoor()
    {
       
        while (Rotation > (0 + OriginalRotation))
        {
            door.localRotation = Quaternion.Euler(0, Rotation, 0);
            Rotation -= speed * Time.deltaTime;

            yield return null;
        }
        isOpen = !isOpen;
    }
}
