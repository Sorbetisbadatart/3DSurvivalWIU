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
    [SerializeField] public Inventory playerInventory;
    [SerializeField] public ItemData woodData;
    [SerializeField] public ItemData rockData;


    public MCFace direction;

    public float offset = 1.0f;
    public float yOffset = 0.1f;
    public float gridSize = 1f;

    public Vector3 extraY = new Vector3(0, 0, 0);

    public bool isbuilding = false;
    public GameObject buildingMenuObj;
    public bool choosingMenuObj;

    void Start()
    {
        currentobject = objects[0];
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (buildingMenuObj.activeSelf)
                DisableMenu();
            else
            {
                EnableMenu();
                StartBuild();
            }
        }

        if (!isbuilding)
        {
            return;
        }

        if (!choosingMenuObj)
            StartPreview();

        if (Input.GetButtonDown("Fire1") && !choosingMenuObj)
        {
            BuildObj();
            //reset placement height of build for new 
            extraY = new Vector3(0, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            CancelBuild();
        }
    }

    public void StartBuild()
    {
        isbuilding = true;
    }

    public void CancelBuild()
    {
        isbuilding = false;
        if (currentpreview != null)
        {
            Destroy(currentpreview.gameObject);
        }
    }
    public void EnableMenu()
    {
        buildingMenuObj.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        choosingMenuObj = true;
    }
    public void DisableMenu()
    {
            buildingMenuObj.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            choosingMenuObj = false;
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
        if (direction == MCFace.Up || direction == MCFace.Down)
        {
            currentpos = hit.point;
        }
        else
        {
            if (direction == MCFace.North)
                currentpos = hit.point + new Vector3(0, 0, 1);
            if (direction == MCFace.South)
                currentpos = hit.point + new Vector3(0, 0, -1);
            if (direction == MCFace.East)
                currentpos = hit.point + new Vector3(1, 0, 0);
            if (direction == MCFace.West)
                currentpos = hit.point + new Vector3(-1, 0, 0);
        }

        if (currentobject.buildingID == Buildings.wall)
        {
            currentpos = hit.point + new Vector3(0, 1, 0);
        }

        currentpos -= Vector3.one * offset;
        currentpos /= gridSize;
        currentpos = new Vector3(Mathf.Round(currentpos.x * 2) / 2, Mathf.Round(currentpos.y * 2) / 2, Mathf.Round(currentpos.z * 2) / 2);
        currentpos /= gridSize;
        currentpos += Vector3.one * offset;

        direction = GetHitFace(hit);

        if (Input.GetKeyDown(KeyCode.UpArrow) && extraY.y < 1)
        {
            extraY += new Vector3(0, 0.5f, 0);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && extraY.y > -1)
        {
            extraY -= new Vector3(0, 0.5f, 0);
        }

        currentpreview.position = currentpos + extraY;


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
            if (playerInventory.CheckItemCount(rockData) < currentobject.StoneCost || playerInventory.CheckItemCount(woodData) < currentobject.WoodCost) 
            {
                Debug.Log("ran out of materials");
                CancelBuild();
                return;
            }
            Debug.Log("vibings");
            AudioManager.Instance.PlaySFX("Build");
            Instantiate(currentobject.buildingPrefab, currentpos + extraY, Quaternion.Euler(currentrotation.x, currentrotation.y, currentrotation.z));
            playerInventory.RemoveItem(rockData, currentobject.StoneCost);
            playerInventory.RemoveItem(woodData, currentobject.WoodCost);
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
    public int WoodCost;
    public int StoneCost;
    public Buildings buildingID;
    public GameObject buildingPrefab;
}
