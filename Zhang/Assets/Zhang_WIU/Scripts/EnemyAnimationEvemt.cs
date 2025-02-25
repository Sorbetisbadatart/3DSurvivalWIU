using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEvemt : MonoBehaviour
{
    [SerializeField] private GameObject _par1;
    [SerializeField] private GameObject _par2;
    private GameObject _currentPar1;
    private GameObject _currentPar2;
    [SerializeField] private Transform _par1Position;
    [SerializeField] private Transform _par2Position;

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
            //_par1.SetActive(false);
            //_par1.SetActive(true);

            Destroy(_currentPar1);
            Vector3 worldPos = _par1Position.transform.position;
            _currentPar1 = Instantiate(_par1, _par1Position.transform.position, _par1Position.transform.rotation, _par1Position.transform);



        }
        else if (a == 2)
        {
            

            _par2.SetActive(false);
            _par2.SetActive(true);

            //Vector3 worldPos = _par2Position.transform.position;
            //_currentPar2 = Instantiate(_par2 , _par2Position.position, Quaternion.identity);
            //_currentPar2 = Instantiate(_par2, _par2Position.transform.position, _par2Position.transform.rotation, transform);

        }

    }

    public void DisableParticle(int a)
    {
        if (a == 1)
        {
            //_par1.SetActive(false);

            if (_currentPar1 != null)
            {
                Destroy(_currentPar1);
                Debug.Log("delete");
            }
        }
        else if (a == 2)
        {
            _par2.SetActive(false);
        }
    }
}
