using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThirstNHunger : MonoBehaviour
{
    [SerializeField] private float thirst;
    [SerializeField]private float hunger;

    [SerializeField] private float thirstRate;
    [SerializeField] private float hungerRate;

    [SerializeField] private Slider hungerSlider;
    [SerializeField] private Slider thirstSlider;

    private PlayerController player;

    [SerializeField] private float dmgTickDelay;
    private float dmgTick;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        hungerSlider.value = hunger;
        thirstSlider.value = thirst;

        if(thirst > 0)
        {
            thirst -= thirstRate * Time.deltaTime;
        }
        if(hunger > 0)
        {
            hunger -= hungerRate * Time.deltaTime;
        }

        if(thirst <= 0)
        {
            //Debug.Log("THIRSTY!");
            dmgTick -= Time.deltaTime;
        }

        if (hunger <= 0)
        {
            //Debug.Log("HUNGRY");
            dmgTick -= Time.deltaTime;
        }

        if(dmgTick <= 0)
        {
            player.SetHealth(player.GetHealth() - 1);
            dmgTick = dmgTickDelay;
            Debug.Log("PLAYER HEALTH: " + player.GetHealth());
        }
    }

    public void GainHunger(float gain)
    {
        hunger += gain;
    }

    public void GainThirst(float gain)
    {
        thirst += gain;
    }
}
