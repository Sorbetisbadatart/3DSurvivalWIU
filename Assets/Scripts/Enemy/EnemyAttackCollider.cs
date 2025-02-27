using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackCollider : MonoBehaviour
{
    [SerializeField] public float _damage;

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            
            Debug.Log($"Damageed from {transform.root.gameObject.name}");
            Debug.Log($"{other.gameObject.name}");


            if (other.gameObject.GetComponent<PlayerController>())
            {
                Debug.Log("Beat the player");
                PlayerController targetController = other.GetComponent<PlayerController>();
                targetController.TakeDamage(1);
            }
        }
    }
}
