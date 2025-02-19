using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingMenu : MonoBehaviour
{
    public List<MenuButton> menuButtons = new List<MenuButton>();
    private Vector2 MousePos;
    private Vector2 VectorToMouse = new Vector2(0.5f, 1.0f);
    private Vector2 CenterCircle = new Vector2(0.5f, 0.5f);
    private Vector2 MouseToVector;

    public int menuItems;
    public int CurrentMenuItem;
    private int OldMenuItem;

    public Build buildSystem;
    // Start is called before the first frame update
    void Start()
    {
        menuItems = menuButtons.Count;
        foreach(MenuButton button in menuButtons)
        {
            button.BuildingImage.color = button.Colour;
        }
        CurrentMenuItem = 0;
        OldMenuItem = 0;
    }

    // Update is called once per frame
    void Update()
    {
        GetCurrentMenuItems();
        if (Input.GetButtonDown("Fire1"))
        {
            ButtonPress();
        }
    }

    public void GetCurrentMenuItems()
    {
        MousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        MouseToVector = new Vector2(MousePos.x/Screen.width, MousePos.y/Screen.height);
        float angle = (Mathf.Atan2(VectorToMouse.y - CenterCircle.y, VectorToMouse.x - CenterCircle.x) - Mathf.Atan2(MouseToVector.y - CenterCircle.y, MouseToVector.x - CenterCircle.x)) * Mathf.Rad2Deg;

        
        if (angle < 0)
        {
            angle += 360;
        }

        CurrentMenuItem = (int)(angle / (360 / menuItems));
        Debug.Log(CurrentMenuItem);

        if (CurrentMenuItem != OldMenuItem)
        {
            menuButtons[OldMenuItem].BuildingImage.color = menuButtons[OldMenuItem].Colour;
            OldMenuItem = CurrentMenuItem;
            menuButtons[OldMenuItem].BuildingImage.color = menuButtons[OldMenuItem].HighLightedColor;
        }
    }

    public void ButtonPress()
    {
        menuButtons[CurrentMenuItem].BuildingImage.color = menuButtons[CurrentMenuItem].SelectedColor;

        buildSystem.ChangeCurrentBuilding (CurrentMenuItem);
        buildSystem.DisableMenu();

    }
}

[System.Serializable]
public class MenuButton
{
    public string BuildingName;
    public Image BuildingImage;
    public Color Colour = Color.white;
    public Color HighLightedColor = Color.grey;
    public Color SelectedColor = Color.grey;

}


