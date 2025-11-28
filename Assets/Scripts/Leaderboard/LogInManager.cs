using PlayFab.ClientModels;
using PlayFab;
using UnityEngine;
using System.Collections.Generic;

public class LogInManager : MonoBehaviour
{

    void Start()
    {
        if (!PlayFabClientAPI.IsClientLoggedIn())
        {
            LogIn();
        }
    }

    void LogIn()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request,
        r => Debug.Log("Score submitted!"),
        e => Debug.LogError(e.GenerateErrorReport()));
    }

    public void SubmitScore(int score)
    {
        var req = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
            new StatisticUpdate { StatisticName = "HighScore", Value = score }
            }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(req,
            r => Debug.Log("Score submitted!"),
            e => Debug.LogError(e.GenerateErrorReport()));
    }

    public void SetPlayerName(string name)
    {
        if (string.IsNullOrEmpty(name)) 
            return;

        var req = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = name.ToUpper()
        };

        PlayFabClientAPI.UpdateUserTitleDisplayName(req,
            r => Debug.Log("Display name set!"),
            e => Debug.LogError(e.GenerateErrorReport()));
    }
}
