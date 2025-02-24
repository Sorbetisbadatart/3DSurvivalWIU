using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEvemt : MonoBehaviour
{
    [SerializeField] private GameObject _par1;
    [SerializeField] private GameObject _par2;

    [SerializeField] private Collider[] detectors;

    private void Start()
    {
        DisableCollider();
    }

    void Update()
    {
        
    }

    public void EnableCollider(int index)
    {
        if (index < 0 || index >= detectors.Length)
        {
            return;
        }

        Collider detector = detectors[index];
        detector.enabled = true;
        //detector.isTrigger = true;
        Debug.Log($"Enable Collider: {detector.gameObject.name}");
        //Debug.Log($"Enable Collider: " + detector.gameObject.name);


    }

    public void DisableCollider()
    {
        foreach (Collider s in detectors)
        {
            s.enabled = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        foreach (Collider detector in detectors)
        {
            if (detector is CapsuleCollider capsule)
            {
                Gizmos.DrawWireSphere(capsule.transform.position, capsule.radius);
            }
            else if (detector is SphereCollider sphere)
            {
                Gizmos.DrawWireSphere(sphere.transform.position, sphere.radius);
            }
            else if (detector is BoxCollider box)
            {
                Gizmos.DrawWireCube(box.transform.position, box.size);
            }
        }
    }


    public void EnableParticle(int a)
    {
        if (a == 1)
        {
            _par1.SetActive(false);
            _par1.SetActive(true);
        }
        else if (a == 2)
        {
            _par2.SetActive(false);
            _par2.SetActive(true);
        }

    }

    public void DisableParticle(int a)
    {
        if (a == 1)
        {
            _par1.SetActive(false);
        }
        else if (a == 2)
        {
            _par2.SetActive(false);
        }
    }
}
