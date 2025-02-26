using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab.ClientModels;
using PlayFab;


public class LBManager : MonoBehaviour
{
    public GameObject scoreRowPrefab;
    public Transform scoreContainer;

    public void AddScore(int rank,string playerName, int score)
    {       
        GameObject oneRow = Instantiate(scoreRowPrefab, scoreContainer);
        TextMeshProUGUI a = oneRow.transform.Find("Name").GetComponent<TextMeshProUGUI>();
        Debug.Log(a);
        oneRow.transform.Find("Rank").GetComponent<TextMeshProUGUI>().text = rank.ToString();
        oneRow.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = playerName;
        oneRow.transform.Find("Score").GetComponent<TextMeshProUGUI>().text = score.ToString();
    }

    private void ClearLeaderBoard()
    {
        foreach (Transform child in scoreContainer)
        {
            Destroy(child.gameObject);
        }
    }
    void Start()
    {     
        OnButtonGetLocalLeaderboard();
    }

    public void OnButtonGetGlobalLeaderboard()
    {
        ClearLeaderBoard();
        var LeaderboardRequest = new GetLeaderboardRequest
        {
            StatisticName = "DaysAlive", //playfab leaderboard stat name
            StartPosition = 0,
            MaxResultsCount = 10
        };

        PlayFabClientAPI.GetLeaderboard(LeaderboardRequest, OnLeaderboardGet, OnError);
    }

    public void OnButtonGetLocalLeaderboard()
    {
        ClearLeaderBoard();
        var LocalLeaderboardRequest = new GetLeaderboardAroundPlayerRequest
        {
            StatisticName = "DaysAlive",
            MaxResultsCount = 5
        };
        PlayFabClientAPI.GetLeaderboardAroundPlayer(LocalLeaderboardRequest, OnLeaderboardAroundPlayerGet, OnError);
    }

    void OnLeaderboardGet(GetLeaderboardResult Result)
    {
        foreach (var item in Result.Leaderboard)
        {
            if (item.DisplayName !=  null)
            AddScore(item.Position + 1,item.DisplayName, item.StatValue);
            else
            AddScore(item.Position + 1, item.PlayFabId, item.StatValue);

        }
     }

    public void OnLeaderboardAroundPlayerGet(GetLeaderboardAroundPlayerResult Result)
    {

        foreach (var item in Result.Leaderboard)
        {
            if (item.DisplayName != null)
                AddScore(item.Position + 1, item.DisplayName, item.StatValue);
            else
                AddScore(item.Position + 1, item.DisplayName, item.StatValue);

        }
       }

    void OnError(PlayFabError Error)
    {
        Debug.Log(Error.GenerateErrorReport());
    }
}


