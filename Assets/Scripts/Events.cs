using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Events : MonoBehaviour
{
    #region CountDown
    [Header("Countdown")]
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private int time;
    [SerializeField] private AudioClip countdownClip;

    public void StartCountDown()
    {
        time = 3;

        StartCoroutine(CountDown());
    }

    private IEnumerator CountDown()
    {
        countdownText.gameObject.SetActive(true);

        NewCountDownValue();
        yield return new WaitForSeconds(1);

        NewCountDownValue();
        yield return new WaitForSeconds(1);

        NewCountDownValue();
        yield return new WaitForSeconds(1);

        NewCountDownValue();
        yield return new WaitForSeconds(1);

        countdownText.gameObject.SetActive(false);
    }

    private void NewCountDownValue()
    {
        AudioManager.instance.PlaySFXClip(countdownClip, "CountDown AudioSource", countdownText.gameObject.transform, 0.5f);

        if (time != 0)
            countdownText.text = time.ToString();
        else
            countdownText.text = "DANCE-OH";

        time--;
    }
    #endregion

    #region Speaker
    [Header("Speaker")]
    [SerializeField] private List<ShockWaveGenerator> speakerList;

    public void SpeakerSummon(string tag)
    {
        foreach (ShockWaveGenerator speaker in speakerList)
        {
            if (speaker.gameObject.tag == tag)
            {
                speaker.enabled = true;
            }
        }
    }
    #endregion

    #region Difficulty
    [Header("Difficulty")]
    [SerializeField] private GameObject difficultyPopUp;
    [SerializeField] private TextMeshProUGUI popUpLabel;
    [SerializeField] private string lastChallenge;

    public void DifficultyUp(float newMultiplier)
    {
        StartCoroutine(DifficultyPopUp());

        // Offset para los intervalos
        float offset = 1.5f;
        float multiplierOffset = newMultiplier / offset;

        if (newMultiplier == 6)
            popUpLabel.text = lastChallenge;

        foreach (ShockWaveGenerator speaker in speakerList)
        {
            // Increase Shock Speed
            speaker.speed = speaker.baseSpeed * newMultiplier;

            // Decrease Spawn Interval
            speaker.lowestValue = speaker.baseLowestValue / (newMultiplier / multiplierOffset);
            speaker.highestValue = speaker.baseHighestValue / (newMultiplier / multiplierOffset);
        }
    }

    private IEnumerator DifficultyPopUp()
    {
        difficultyPopUp.SetActive(true);

        yield return new WaitForSeconds(2f);

        difficultyPopUp.SetActive(false);
    }
    #endregion

    public void EndGame()
    {
        GameManager.instance.EndGame(true);
    }
}