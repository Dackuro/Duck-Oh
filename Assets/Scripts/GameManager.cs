using TMPro;
using UnityEngine;
using System.Collections;
using static System.TimeZoneInfo;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // General
    bool gamePaused;

    [Header("References")]
    [SerializeField] private InputManager inputManager;
    [SerializeField] LogInManager logInManager;

    public bool gameStarted = false;
    public bool gameEnded = false;
    public bool stillPlaying = false;

    #region GameStart
    [Header("GameStart")]
    [SerializeField] private GameObject tutorialPanel;
    #endregion

    #region End Manager
    [Header("End Manager")]
    [Header("References")]
    [SerializeField] private GameObject endGameMenu;
    [Space]
    [SerializeField] private TextMeshProUGUI menuLabel;
    [SerializeField] private TextMeshProUGUI pointsLabel;
    [SerializeField] private TextMeshProUGUI newRecordLabel;
    [Space]
    [SerializeField] private GameObject retryButton;
    [SerializeField] private GameObject keepPlayButton;

    [Header("Texts")]
    [SerializeField] private string winText = "You Win";
    [SerializeField] private string loseText = "You Lose";

    [Header("Colors")]
    [SerializeField] private Color winColor = Color.green;
    [SerializeField] private Color loseColor = Color.red;
    #endregion

    #region Timer
    [Header("Timer")]
    [Header("Displays")]
    [SerializeField] private TextMeshProUGUI rightDisplay;
    [SerializeField] private TextMeshProUGUI leftDisplay;
    [Space]
    [SerializeField] private float time;
    #endregion


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        gameStarted = false;
        gameEnded = false;
        stillPlaying = false;
    }

    private void Update()
    {
        Timer();
    }


    #region Pause
    public void TogglePause(string menu)
    {
        gamePaused = !gamePaused;

        // Menu
        switch (menu.ToLower().Trim())
        {
            case "pause":
                MainMenu.instance.MainMenuToggle(gamePaused);
                break;

            case "endgame":
                endGameMenu.SetActive(true);
                break;

            case "keep":
                endGameMenu.SetActive(false);
                stillPlaying = true;
                gameEnded = false;
                break;

            default:
                break;
        }          
        
        // Time
        Time.timeScale = gamePaused ? 0f : 1f;

        // Inputs
        inputManager.EnableInputs(!gamePaused);

        // Cursor
        Cursor.visible = gamePaused;
        Cursor.lockState = gamePaused ? CursorLockMode.Confined : CursorLockMode.Locked;     
    }
    #endregion

    #region Timer
    public void Timer()
    {
        if (!gameStarted)
            return;

        time += Time.deltaTime;

        // Calculator
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        // Displays
        string formattedText = string.Format("{0:00}:{1:00}", minutes, seconds);
        rightDisplay.text = formattedText;
        leftDisplay.text = formattedText;
    }
    #endregion

    #region GameStart
    public void StartGame()
    {
        gameStarted = true;

        if (tutorialPanel)
            tutorialPanel.SetActive(false);
    }
    #endregion

    #region GameEnd
    public void EndGame(bool isWin = false)
    {
        gameEnded = true;

        SubmitScore();

        TogglePause("endgame");

        // Text & Color
        menuLabel.text = isWin ? winText: loseText;       
        menuLabel.color = isWin ? winColor: loseColor;

        // Buttons
        if (!stillPlaying)
        {
            retryButton.SetActive(isWin ? false : true);
            keepPlayButton.SetActive(isWin ? true : false);
        }
        else
        {
            retryButton.SetActive(true);
            keepPlayButton.SetActive(false);
        }
        
        // Points
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        // Arreglo cutre
        if (minutes == 2 && seconds == 59)
        {
            minutes = 03;
            seconds = 00;
        }

        pointsLabel.text = time.ToString();
        pointsLabel.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (IsNewRecord())
        {
            PlayerPrefs.SetFloat("Record", time);

            StartCoroutine(NewRecordRainbow());
        }
    }

    public bool IsNewRecord()
    {
        float currentRecord = PlayerPrefs.GetFloat("Record");
        bool newRecord = true;

        if (currentRecord >= time)
            newRecord = false;

        return newRecord;
    }

    private IEnumerator NewRecordRainbow()
    {
        newRecordLabel.gameObject.SetActive(true);

        // Color
        float transitionTime = 0.2f;

        // Size
        float pulseSpeed = 3f;
        float minScale = 0.8f;
        float maxScale = 1.2f;

        Vector3 baseScale = newRecordLabel.transform.localScale;

        while (true)
        {
            // Color
            Color startColor = newRecordLabel.color;
            Color targetColor = new Color(Random.value, Random.value, Random.value);

            while (targetColor == startColor)            
                targetColor = new Color(Random.value, Random.value, Random.value);
            
            float t = 0f;

            while (t < 1f)
            {
                // Color
                t += Time.unscaledDeltaTime / transitionTime;
                newRecordLabel.color = Color.Lerp(startColor, targetColor, t);

                // Size
                float pulse = (Mathf.Sin(Time.unscaledTime * pulseSpeed) + 1f) / 2f;
                float scale = Mathf.Lerp(minScale, maxScale, pulse);
                newRecordLabel.transform.localScale = baseScale * scale;

                yield return null;
            }

            //yield return new WaitForSecondsRealtime(0.1f);
        }
    }
    #endregion



    public void SubmitScore()
    {
        int pointAmount = Mathf.RoundToInt(time);
        logInManager.SubmitScore(pointAmount);
    }
}