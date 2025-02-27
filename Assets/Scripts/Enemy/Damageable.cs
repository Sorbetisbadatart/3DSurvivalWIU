using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    private float health = 100f;
    [SerializeField] public GameObject brokenPrefab;
    [SerializeField] public ParticleSystem breakParticlePrefab;


    //create hit effect anim
    private Color originalColour;
    public Color damageColour = Color.red;
    private float damageDuration = 0.5f;
    private Renderer renderer;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        originalColour =  renderer.material.color;
        
      
    }

    private IEnumerator DamageEffect()
    {
        renderer.material.color = damageColour;
        float elapseTime = 0.0f;
        while(elapseTime <= damageDuration)
        {
            renderer.material.color = Color.Lerp(damageColour, originalColour, elapseTime / damageDuration);
            elapseTime += Time.deltaTime;
            yield return null;
        }
       renderer.material.color = originalColour;
    }

    public void TakeDamage(float amount)
    {

        if (renderer != null)
        {
            StartCoroutine(DamageEffect());
        }
        Debug.Log("ouch");
        health -= amount;
        if (health <= 0)
        {
            Debug.Log("health: " + health);
            Instantiate(brokenPrefab, transform.position, Quaternion.Euler(0,0,0));
            Instantiate(breakParticlePrefab, transform.position, Quaternion.Euler(0, 0, 0));
            Destroy(gameObject);
           
        }
    }
   
}
        
    

