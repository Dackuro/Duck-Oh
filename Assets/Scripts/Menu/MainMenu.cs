using UnityEngine;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(MenuCollection))]
public class MainMenu : MonoBehaviour
{
    public static MainMenu instance;

    [SerializeField] private MenuCollection collection;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {        
        // Cursor
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;          
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        // Menus
        if (collection.optionsMenu) collection.optionsMenu.SetActive(false);
        if (collection.audioMenu) collection.audioMenu.SetActive(false);
        if (collection.controlsMenu) collection.controlsMenu.SetActive(false);

        if (collection.exitPopUp) collection.exitPopUp.SetActive(false);
    }

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }


    #region MainMenu
    public void MainMenuToggle(bool isActive)
    {
        collection.mainMenu.SetActive(isActive);

        if (collection.optionsMenu) collection.optionsMenu.SetActive(false);
        if (collection.audioMenu) collection.audioMenu.SetActive(false);
        if (collection.controlsMenu) collection.controlsMenu.SetActive(false);

        if (!isActive)
            if (collection.exitPopUp) collection.exitPopUp.SetActive(false);
    }
    #endregion

    #region Options
    public void OptionsMenu()
    {
        bool isActive = collection.optionsMenu.activeSelf;
        collection.optionsMenu.SetActive(!isActive);

        if (collection.mainMenu) collection.mainMenu.SetActive(isActive);
        if (collection.audioMenu) collection.audioMenu.SetActive(false);
        if (collection.controlsMenu) collection.controlsMenu.SetActive(false);

        if (!isActive)
            if (collection.exitPopUp) collection.exitPopUp.SetActive(false);
    }
    #endregion

    #region Audio
    public void AudioMenu()
    {
        bool isActive = collection.audioMenu.activeSelf;
        collection.audioMenu.SetActive(!isActive);

        if (collection.mainMenu) collection.mainMenu.SetActive(isActive);
        if (collection.optionsMenu) collection.optionsMenu.SetActive(false);
        if (collection.controlsMenu) collection.controlsMenu.SetActive(false);

        if (!isActive)
            if (collection.exitPopUp) collection.exitPopUp.SetActive(false);
    }
    #endregion

    #region Exit
    public void PopUpExit()
    {
        bool isActive = collection.exitPopUp.activeSelf;
        collection.exitPopUp.SetActive(!isActive);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void CancelExit()
    {
        collection.exitPopUp.SetActive(false);
    }
    #endregion
}
