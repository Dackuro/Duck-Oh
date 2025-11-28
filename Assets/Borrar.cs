using UnityEngine;
using TMPro;

public class LogIn : MonoBehaviour
{
    public LogInManager logInManager;
    public LeaderBoardUI leaderBoardUI;

    public TMP_InputField nameInput;
    public TMP_InputField scoreInput;

    public void SaveName(string newName)
    {
        if (newName.Length < 4)
        {
            Debug.LogError("Name too small");
            return;
        }

        logInManager.SetPlayerName(newName);

        Debug.Log("New name set");
    }

    public void SavePoints()
    {
        if (int.TryParse(scoreInput.text, out int score))
        {
            logInManager.SubmitScore(score);
        }
    }

    public void RefreshPoints()
    {
        leaderBoardUI.ShowLeaderboard();
    }
}
