using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlacementGenerator : MonoBehaviour
{
    [SerializeField] List<GameObject> preFab;

    [Header("Raycast Settings")]
    [SerializeField] int density;

    [Space]

    [SerializeField] float minHeight;
    [SerializeField] float maxHeight;
    [SerializeField] Vector2 xRange;
    [SerializeField] Vector2 zRange;

    [Header("Prefab variation settings")]
    [SerializeField, Range(0, 1)] float rotateTowardsNormal;
    [SerializeField] Vector2 rotationRange;
    [SerializeField] Vector3 minScale;
    [SerializeField] Vector3 maxScale;


    private void Start()
    {
        Generate();
    }
    private void Update()
    {
        // Testing purposes
        if (Input.GetKeyDown(KeyCode.Alpha1))
            Clear();
        if (Input.GetKeyDown(KeyCode.Alpha2))
            Generate();
        if (Input.GetKeyDown(KeyCode.Alpha3))
            SetVisibility(true);
        if (Input.GetKeyDown(KeyCode.Alpha4))
            SetVisibility(false);
    }
    public void Generate()
    {
        int selectedObject;
        xRange += new Vector2(transform.position.x,transform.position.z);
        zRange += new Vector2(transform.position.x, transform.position.z);
        for (int i =0;i<density; i++)
        {
            float sampleX = Random.Range(xRange.x,xRange.y);
            float sampleZ = Random.Range(zRange.x,zRange.y);
            Vector3 rayStart = new Vector3(sampleX,maxHeight,sampleZ);

            if (!Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, Mathf.Infinity))
                continue;

            if (hit.point.y < minHeight)
                continue;

            selectedObject = Random.Range(0, preFab.Count);

            GameObject instantiatedPrefab = (GameObject)PrefabUtility.InstantiatePrefab(this.preFab[selectedObject], transform);
            instantiatedPrefab.transform.position = hit.point;
            instantiatedPrefab.transform.Rotate(Vector3.up, Random.Range(rotationRange.x, rotationRange.y), Space.Self);
            instantiatedPrefab.transform.rotation = Quaternion.Lerp(transform.rotation, transform.rotation * Quaternion.FromToRotation(instantiatedPrefab.transform.up, hit.normal), rotateTowardsNormal);
            instantiatedPrefab.transform.localScale = new Vector3(
                Random.Range(minScale.x, maxScale.x),
                Random.Range(minScale.y, maxScale.y),
                Random.Range(minScale.z, maxScale.z)
                );
        }
    }

    public void Clear()
    {
        while (transform.childCount != 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }

    public void SetVisibility(bool visibility){
        foreach(Transform child in this.transform){
            child.gameObject.SetActive(visibility);
        }
    }
    
   
}
