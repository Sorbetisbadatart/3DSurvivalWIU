using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public static TimeController Timeinstance;
    [SerializeField] private float timeMultiplier;

    [SerializeField] private float startHour;

    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI dayText;

    public static DateTime currentTime;
    private TimeSpan sunriseTime;
    private TimeSpan sunsetTime;

    [SerializeField] private Light sun;
    [SerializeField] private float sunriseHour;
    [SerializeField] private float sunsetHour;

    [SerializeField] private Color DayAmbientLight;
    [SerializeField] private Color NightAmbientLight;

    [SerializeField] private AnimationCurve LightChangeCurve;

    [SerializeField] private float maxSunLightIntensity;

    [SerializeField] private Light moonLight;
    [SerializeField] private float maxMoonLightIntensity;

    private int currentDay = 0;

    bool calledonce = false;

    private void Awake()
    {
        if (Timeinstance == null)
        {
            Timeinstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentTime = DateTime.Now.Date + TimeSpan.FromHours(startHour);

        sunriseTime = TimeSpan.FromHours(sunriseHour);
        sunsetTime = TimeSpan.FromHours(sunsetHour);
    }



    // Update is called once per frame
    void Update()
    {
        UpdateTimeOfDay();
        RotateSun();
        UpdateLightSettings();
        //SummonRooster();
        TimePassedThisTime(2);
        DayPassed();

    }

    public void DayPassed()
    {
        if (currentTime.Hour == 0)
        {
            currentDay++;
            PFUserManager.instance.SendLeaderboard(currentDay);
            calledonce = false;
        }
    }

    public int GetCurrentTimeinHours()
    {
        return currentTime.Hour;
    }

    public int GetCurrentTimeinSeconds()
    {
        return currentTime.Second;
    }
    
    //public bool TimePassedThisTime(int TimeToPassinHours)
    //{
    //    //dont put 0 (maybe 1)
    //    if (GetCurrentTimeinHours() == TimeToPassinHours && !calledonce)
    //    {          
    //        calledonce = true;
    //        return calledonce;      
    //    }
    //    //Debug.Log("passedaway");
    //    return false;
    //}

    public bool TimePassedThisTime(int TimeToPassinHours)
    {
        //dont put 0 (maybe 1)
        if (GetCurrentTimeinHours() == TimeToPassinHours)
        {
        
            return true;
        }
        //Debug.Log("passedaway");
        return false;
    }

    private void UpdateTimeOfDay()
    {
        currentTime = currentTime.AddSeconds(timeMultiplier * Time.deltaTime);

        

        if (timeText != null )
        {
           timeText.text = currentTime.ToString("HH:mm");
           dayText.text = (currentTime - DateTime.Now.Date).ToString("dd") ;
           if (currentTime.TimeOfDay >= TimeSpan.FromHours(sunriseHour) && currentTime.TimeOfDay <= TimeSpan.FromHours(sunriseHour + 0.01) )
           {
                
                SummonRooster() ;
           }
        }

       
    }

    private void SummonRooster()
    {     
            AudioManager.Instance.PlaySFX("Rooster");      
    }

    private void RotateSun()
    {
        float sunLightRotation;

        if (currentTime.TimeOfDay > sunriseTime && currentTime.TimeOfDay < sunsetTime)
        {
            TimeSpan sunriseToSunsetDuration = CalculateTimeDifference(sunriseTime, sunsetTime);
            TimeSpan timeSinceSunrise = CalculateTimeDifference(sunriseTime, currentTime.TimeOfDay);

            double percentage = timeSinceSunrise.TotalMinutes / sunriseToSunsetDuration.TotalMinutes;

            sunLightRotation = Mathf.Lerp(0, 180, (float)percentage);
        }
        else
        {
            TimeSpan sunsetToSunriseDuration = CalculateTimeDifference(sunsetTime, sunriseTime);
            TimeSpan timeSinceSunrise = CalculateTimeDifference(sunsetTime, currentTime.TimeOfDay);

            double percentage = timeSinceSunrise.TotalMinutes / sunsetToSunriseDuration.TotalMinutes;
            sunLightRotation = Mathf.Lerp(180, 360, (float)percentage);
        }
        sun.transform.rotation = Quaternion.AngleAxis(sunLightRotation, Vector3.right);
    }

    private TimeSpan CalculateTimeDifference(TimeSpan fromTime, TimeSpan toTime)
    {
        TimeSpan difference = toTime - fromTime;

        if (difference.TotalSeconds < 0 ) {
            difference += TimeSpan.FromHours(24);
        }
        return difference;
    }

    

    private void UpdateLightSettings()
    {
        float dotProduct = Vector3.Dot(sun.transform.forward, Vector3.down);
        sun.intensity = Mathf.Lerp(0,maxSunLightIntensity, LightChangeCurve.Evaluate (dotProduct));
        moonLight.intensity = Mathf.Lerp(maxMoonLightIntensity, 0, LightChangeCurve.Evaluate(dotProduct));
        RenderSettings.ambientLight = Color.Lerp(NightAmbientLight, DayAmbientLight, LightChangeCurve.Evaluate(dotProduct));
    }
}
