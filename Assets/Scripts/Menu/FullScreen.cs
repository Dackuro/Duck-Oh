using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

public class FullScreen : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private bool isFullScreen;


    private void Start()
    {
        isFullScreen = (PlayerPrefs.GetInt("isFullScreen") != 0);

        Screen.fullScreen = isFullScreen;
        label.text = isFullScreen ? "Full Screen" : "Windowed";
    }

    public void ToggleScreenMode()
    {
        isFullScreen = !isFullScreen;

        // Screen Mode
        Screen.fullScreen = isFullScreen;
        int value = isFullScreen ? 1 : 0;
        PlayerPrefs.SetInt("isFullScreen", value);

        // Label
        label.text = isFullScreen ? "Full Screen" : "Windowed";

        // Cursor
        Cursor.lockState = isFullScreen ? CursorLockMode.Confined : CursorLockMode.None;
    }
}
