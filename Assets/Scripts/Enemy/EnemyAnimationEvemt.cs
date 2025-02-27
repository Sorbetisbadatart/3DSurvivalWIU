using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class EnemyAnimationEvemt : MonoBehaviour
{
    [SerializeField] private GameObject _par1;
    [SerializeField] private GameObject _par2;
    private GameObject _currentPar1;
    private GameObject _currentPar2;
    [SerializeField] private Transform _par1Position;
    [SerializeField] private Transform _par2Position;

    [SerializeField] private Collider[] detectors;
   
   


    [SerializeField] private GameObject _tailTrail;
    [SerializeField] private GameObject _rightTrail;
    [SerializeField] private GameObject _leftTrail;

    AudioSource _audioSource;
    [SerializeField] private AudioClip attack1, attack2, attack3, attack4, attack5, _a, _b, _c, _footStep;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();

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

    public void EnableTrail(int a)
    {
        switch (a)
        {
            case 1:
                _leftTrail.SetActive(true);
                break;
            case 2:
                _rightTrail.SetActive(true);
                break;
            case 3:
                _tailTrail.SetActive(true);
                break;
        }
    }

    public void DisableTrail(int a)
    {
        switch (a)
        {
            case 1:
                _leftTrail.SetActive(false);
                break;
            case 2:
                _rightTrail.SetActive(false);
                break;
            case 3:
                _tailTrail.SetActive(false);
                break;
        }
    }

    public void EnableSFX(int a)
    {
        switch (a)
        {
            case 1:
                _audioSource.PlayOneShot(attack1);
                break;
            case 2:

                break;
            case 3:
                _audioSource.PlayOneShot(_a);
                _audioSource.PlayOneShot(attack2);
                break;
            case 4:
                _audioSource.PlayOneShot(_b);
                break;
            case 5:
                _audioSource.PlayOneShot(_c);
                break;
            case 6:
                _audioSource.PlayOneShot(_footStep);
                break;
        }
    }
}
