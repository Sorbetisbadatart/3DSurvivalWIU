using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirstNHunger : MonoBehaviour
{
    [SerializeField] private float thirst;
    [SerializeField]private float hunger;

    [SerializeField] private float thirstRate;
    [SerializeField] private float hungerRate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(thirst > 0)
        {
            thirst -= thirstRate * Time.deltaTime;
        }
        if(hunger > 0)
        {
            hunger -= hungerRate * Time.deltaTime;
        }

        if(thirst <= 0)
        {
            Debug.Log("THIRSTY!");
        }

        if (hunger <= 0)
        {
            Debug.Log("HUNGRY");
        }
    }
}
