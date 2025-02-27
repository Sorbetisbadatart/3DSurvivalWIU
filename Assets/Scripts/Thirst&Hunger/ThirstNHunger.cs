
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
        dmgTick = dmgTickDelay;
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
            dmgTick -= Time.deltaTime;
        }

        if (hunger <= 0)
        {
           dmgTick -= Time.deltaTime;
        }

        if(dmgTick <= 0)
        {
            player.TakeDamage(5);
            dmgTick = dmgTickDelay;
    
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
