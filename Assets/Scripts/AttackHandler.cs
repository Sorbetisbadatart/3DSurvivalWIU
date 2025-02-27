using Cinemachine;
using UnityEngine;
public class AttackHandler : MonoBehaviour
{
    [SerializeField] private SphereCollider[] detectors;
    [SerializeField] private  SphereCollider _collider;

    [SerializeField] private CinemachineImpulseSource source;
    [SerializeField] private string Damageablelayer = "Damagable";


    private void Start()
    {
        DisableCollider();
    }
    public void EnableCollider()
    {
        foreach (SphereCollider sCollider in detectors)
        {
            sCollider.enabled = true;
        }

        // Only detect collision with any object on Target layer
        LayerMask layer = LayerMask.GetMask(Damageablelayer);
        if (_collider.enabled)
        {
            foreach (SphereCollider sCollider in detectors)
            {
                Debug.Log(3);
                Collider[] hitColliders =
                Physics.OverlapSphere(sCollider.transform.position,
                sCollider.radius, layer);
                for (int i = 0; i < hitColliders.Length; i++)
                {
                    _collider.enabled = false;
                    Debug.Log(hitColliders[i].gameObject.name);


                    // Perform damage on other object, show feedback, etc

                    if (hitColliders[i].gameObject.GetComponent<EnemyController>())
                    {
                        EnemyController targetController = hitColliders[i].GetComponent<EnemyController>();
                        targetController.TakeDamage(1);
                    }

                    if (hitColliders[i].gameObject.GetComponent<Animals>())
                    {

                        Animals animalController = hitColliders[i].GetComponent<Animals>();
                        animalController.TakeDamage(1);
                    }




                    // Step 5 - Generate Impulse
                    source.GenerateImpulse(Camera.main.transform.forward);
                }
            }
        }
    }
    public void DisableCollider()
    {
        foreach (SphereCollider sCollider in detectors)
        {
            sCollider.enabled = false;
        }
    }


   

    private void OnDrawGizmos()
    {
        foreach (SphereCollider sCollider in detectors)
        {
            sCollider.enabled = false;
        }
    }
}
