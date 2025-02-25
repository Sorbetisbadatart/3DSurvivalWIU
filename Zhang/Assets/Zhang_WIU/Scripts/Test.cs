using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public EnemyController _enemyController;

    //public Animals _animals;
    void Start()
    {
        //_animals = FindObjectOfType<Animals>();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Q))
        {
            _enemyController.TakeDamage(1.0f);
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            //_animals.TakeDamage(1.0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if(other.gameObject.tag== "Animal1")
        //{
        //    _animals.TakeDamage(1.0f);
        //}
        if(other.gameObject.TryGetComponent<Animals>(out Animals Animals))
        {
            Animals.TakeDamage(1.0f);
        }
        
    }
}
