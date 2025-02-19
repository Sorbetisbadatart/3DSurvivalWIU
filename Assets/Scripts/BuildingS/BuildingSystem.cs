using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Build : MonoBehaviour
{
    public enum MCFace
    {
        None,
        Up,
        Down,
        East,
        West,
        North,
        South
    }

    public List<BuildObjects> objects = new List<BuildObjects>();
    public BuildObjects currentobject;
    private Vector3 currentpos;
    private Vector3 currentrotation;
    public Transform currentpreview;
    public Transform cam;
    public RaycastHit hit;
    public LayerMask layer;

    public MCFace direction;

    public float offset = 1.0f;
    public float yOffset = 0.1f;
    public float gridSize = 1f;

    public bool isbuilding;
    public GameObject buildingMenuObj;
    public bool choosingMenuObj;




    void Start()
    {
        currentobject = objects[0];
        ChangeCurrentBuilding(1);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (isbuilding && !choosingMenuObj)
            StartPreview();

        if (Input.GetKeyDown(KeyCode.Space) && !choosingMenuObj)
        {
            BuildObj();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (buildingMenuObj.activeSelf)
                DisableMenu();
            else
                EnableMenu();


        }





    }

    public void EnableMenu()
    {
        buildingMenuObj.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        choosingMenuObj = true;
    }

    public void DisableMenu()
    {
        {
            buildingMenuObj.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            choosingMenuObj = false;

        }
    }





    public void ChangeCurrentBuilding(int BuildingID)
    {
        currentobject = objects[BuildingID];
        if (currentpreview != null)
        {
            Destroy(currentpreview.gameObject);
        }
        GameObject curprev = Instantiate(currentobject.preview, currentpos, Quaternion.Euler(currentrotation.x, currentrotation.y, currentrotation.z)) as GameObject;
        currentpreview = curprev.transform;
    }

    public void StartPreview()
    {
        if (Physics.Raycast(cam.position, cam.forward, out hit, 10, layer))
        {
            if (hit.transform != this.transform)
                ShowPreview(hit);
        }
    }

    public void ShowPreview(RaycastHit hit)
    {
        if (currentobject.buildingID == Buildings.floor)
        {
            direction = GetHitFace(hit);
            if (direction == MCFace.Up || direction == MCFace.Down)
            {
                currentpos = hit.point;
            }
            else
            {
                if (direction == MCFace.North)
                    currentpos = hit.point + new Vector3(0, 0, 2);
                if (direction == MCFace.South)
                    currentpos = hit.point + new Vector3(0, 0, -2);
                if (direction == MCFace.East)
                    currentpos = hit.point + new Vector3(2, 0, 0);
                if (direction == MCFace.West)
                    currentpos = hit.point + new Vector3(-2, 0, 0);
            }
        }
        else

            currentpos = hit.point;
        currentpos -= Vector3.one * offset;
        currentpos /= gridSize;
        currentpos = new Vector3(Mathf.Round(currentpos.x * 2) / 2, Mathf.Round(currentpos.y  * 2) / 2, Mathf.Round(currentpos.z * 2)/2);
        currentpos /= gridSize;
        currentpos += Vector3.one * offset;
        currentpos += Vector3.up * yOffset;
        currentpreview.position = currentpos;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentrotation += new Vector3(0, 45, 0);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentrotation += new Vector3(0, -45, 0);
        }
        currentpreview.localEulerAngles = currentrotation;
    }

    public void BuildObj()
    {
        PreviewObject _previewObject = currentpreview.GetComponent<PreviewObject>();
        if (_previewObject.canBuild)
        {
            Instantiate(currentobject.buildingPrefab, currentpos, Quaternion.Euler(currentrotation.x, currentrotation.y, currentrotation.z));

        }
    }

    public static MCFace GetHitFace(RaycastHit hit)
    {
        Vector3 incomingVector = hit.normal - Vector3.up;

        if (incomingVector == new Vector3(0, -1, -1))
        {
            return MCFace.South;
        }
        if (incomingVector == new Vector3(0, -1, 1))
        {
            return MCFace.North;
        }
        if (incomingVector == new Vector3(0, 0, 0))
        {
            return MCFace.Up;
        }
        if (incomingVector == new Vector3(1, 1, 1))
        {
            return MCFace.Down;
        }
        if (incomingVector == new Vector3(-1, -1, 0))
        {
            return MCFace.West;
        }
        if (incomingVector == new Vector3(1, -1, 0))
        {
            return MCFace.East;
        }

        return MCFace.None;
    }



}
[System.Serializable]
public class BuildObjects
{
    public string name;
    public GameObject preview;
    public Buildings buildingID;
    public GameObject buildingPrefab;
    public int gold;
}
