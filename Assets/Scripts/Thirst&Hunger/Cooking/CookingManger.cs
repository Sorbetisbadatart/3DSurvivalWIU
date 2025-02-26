using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookingManger : MonoBehaviour
{
    [SerializeField] private GameObject cookingList;
    [SerializeField] private GameObject cookingMinigame;

    [SerializeField] private Slider sliderGame;

    private Inventory inventory;

    //cooking minigame
    private bool inCooking;
    private bool sliderRight;
    private bool gameResult;

    public FoodData foodResult;

    // Start is called before the first frame update
    void Start()
    {
        inventory = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>().GetInventory();
    }

    // Update is called once per frame
    void Update()
    {
        if(inCooking)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                gameResult = true;
            }

            if(!gameResult)
            {
                AudioManager.Instance.PlaySFX("Cooking");
                //slider move left and right until player inputs space
                if (sliderRight)
                {
                    sliderGame.value += Time.deltaTime;

                    if (sliderGame.value >= sliderGame.maxValue)
                    {
                        sliderRight = false;
                    }
                }
                else
                {
                    sliderGame.value -= Time.deltaTime;

                    if (sliderGame.value <= sliderGame.minValue)
                    {
                        sliderRight = true;
                    }
                }
            }
            else
            {
                //game end
                ItemInstance food = new ItemInstance(foodResult);
                int count = 1;
                //give result
                //yellow zone
                if(sliderGame.value>=0.275f && sliderGame.value <= 0.725f)
                {
                    count++;
                    //green zone
                    if(sliderGame.value >= 0.425f && sliderGame.value <= 0.575f)
                    {
                        count++;
                    }
                }

                //if not in zone just give base item
                inventory.AddItem(food, count);

                cookingList.SetActive(true);
                cookingMinigame.SetActive(false);

                gameResult = false;
                inCooking = false;
            }
        }
    }

    public void SetCookingListActive(bool value)
    {
        cookingList.SetActive(value);

        if(value)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void StartCookingMinigame(FoodData food)
    {
        cookingList.SetActive(false);
        cookingMinigame.SetActive(true);

        foodResult = food;

        sliderGame.value = 0;
        gameResult = false;
        sliderRight = true;
        inCooking = true;
    }
}
