using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

using TMPro;
using UnityEngine;
using System;


public class PFUserManager : MonoBehaviour
{
    public static  PFUserManager instance;
  
    private void Start()
    {
        if (instance == null)
            instance = this;

       
        
    }

    public void OnButtonGuestLogin()
    {
        var request = new LoginWithCustomIDRequest { CustomId = "GuestUser",  CreateAccount = true};
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnError);
        
    }

    void OnError(PlayFabError Error)
    {
        Debug.Log(Error.GenerateErrorReport());
    }

    void OnLoginSuccess(LoginResult result)
    {

        Invoke(nameof(LoadLevel), 2);
    }

    public void SendLeaderboard(int Score)
    {
        var Request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "DaysAlive",
                    Value = Score,

                }

            }
        };

        
    }

    public void OnLeaderboardUpdate(UpdatePlayerStatisticsResult Result)
    {
    }

    public void LoadLevel()
    {
        SceneManager.LoadScene(1);
    }   
    
}
