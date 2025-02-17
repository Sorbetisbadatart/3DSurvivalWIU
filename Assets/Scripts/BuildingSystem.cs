
using System.Collections.Generic;
using UnityEngine;

public class Build : MonoBehaviour
{

    public List<BuildObjects> objects = new List<BuildObjects> ();
    public BuildObjects currentobject;
    private Vector3 currentpos;
    public Transform currentpreview;
    public Transform cam;
    public RaycastHit hit;
    public LayerMask layer;

    public float offset = 1.0f;
    public float gridSize = 0.0f;

    public bool isbuilding;

    void Start()
    {
        currentobject = objects [0];
        ChangeCurrentBuilding ();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if(isbuilding)
        StartPreview();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            BuildObj();
        }
        
    }

    public void ChangeCurrentBuilding()
    {
        GameObject curprev = Instantiate (currentobject.preview, currentpos, Quaternion.identity) as GameObject;
        currentpreview = curprev.transform;
    }

    public void StartPreview()
    {
        if(Physics.Raycast(cam.position, cam.forward, out hit, 10, layer))
        {
            if(hit.transform != this.transform)
                ShowPreview(hit);
        }
    }

    public void ShowPreview(RaycastHit hit2)
    {
        currentpos = hit2.point;
        currentpos -= Vector3.one * offset;
        currentpos /= gridSize;
        currentpos = new Vector3(Mathf.Round(currentpos.x), Mathf.Round(currentpos.y), Mathf.Round(currentpos.z));
        currentpos *= gridSize;
        currentpos += Vector3.one * offset;
        currentpreview.position = currentpos;
    }

    public void BuildObj()
    {
        PreviewObject _previewObject = currentpreview.GetComponent<PreviewObject>();
        if (_previewObject.canBuild)
        {
            Instantiate(currentobject.buildingPrefab,currentpos,Quaternion.identity);
            
        }
    }

}
[System.Serializable]
public class BuildObjects
{
    public string name;
    public GameObject preview;
    public GameObject buildingPrefab;
    public int gold;
}