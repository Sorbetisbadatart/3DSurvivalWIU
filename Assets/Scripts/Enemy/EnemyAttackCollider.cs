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
            //Enemies player = other.GetComponent<Enemies>();
            //if (player != null)
            //{
            //    player.TakeDamage(_damage, transform.root.gameObject); //
            //}
            Debug.Log($"Damageed from {transform.root.gameObject.name}");
            Debug.Log($"{other.gameObject.name}");
        }
    }
}
