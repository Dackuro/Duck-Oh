using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections;

public class LeaderBoardUI : MonoBehaviour
{
    [Header("Leaderboard")]
    public GameObject entryPrefab;
    public Transform contentParent;

    [Header("LogIn")]
    public LogInManager logInManager;
    public GameObject logInMenu;
    public GameObject errorPopUp;

    private void Start()
    {
        if (!PlayFabClientAPI.IsClientLoggedIn())
            logInMenu.SetActive(true);        
        else
        {
            logInMenu.SetActive(false);

            StartCoroutine(DelayedStart());
        }
    }

    public IEnumerator DelayedStart()
    {
        while (!PlayFabClientAPI.IsClientLoggedIn())
        {
            yield return null; // wait until logged in
        }

        ShowLeaderboard();
    }

    public void ShowLeaderboard()
    {
        PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest
        {
            StatisticName = "HighScore",
            StartPosition = 0,
            MaxResultsCount = 7
        },
        result =>
        {
            foreach (Transform child in contentParent)
                Destroy(child.gameObject);

            foreach (var entry in result.Leaderboard)
            {               
                GameObject go = Instantiate(entryPrefab, contentParent);
                TextMeshProUGUI text = go.GetComponent<TextMeshProUGUI>();
                text.text = $"{entry.Position + 1}. {entry.DisplayName} - {FormatTime(entry.StatValue)}";

                if (entry.PlayFabId == PlayFabSettings.staticPlayer.PlayFabId)
                    text.color = Color.yellow;              
            }
        },
        error => Debug.LogError(error.GenerateErrorReport()));
    }

    private string FormatTime(int totalSeconds)
    {
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        return $"{minutes:00}:{seconds:00}";
    }

    #region LogIn
    public void SaveName(string newName)
    {
        if (newName.Length < 4)
            StartCoroutine(ErrorPopUp());
        else
        {
            logInManager.SetPlayerName(newName);
            logInMenu.SetActive(false);

            StartCoroutine(DelayedStart());
        }
    }

    private IEnumerator ErrorPopUp()
    {
        errorPopUp.SetActive(true);

        yield return new WaitForSecondsRealtime(1.5f);

        errorPopUp.SetActive(false);
    }
    #endregion
}
