using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance;


    [Header("General")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Mute")]
    [SerializeField] private Toggle toggleSilenciar;
    [SerializeField] private Image toggleImage;
    [SerializeField] private Image soundImage;
    [SerializeField] private Sprite unmutedImage;
    [SerializeField] private Sprite mutedImage;

    [Header("Sliders")]
    [SerializeField] private Slider mainSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;

    [Header("Text References")]
    [SerializeField] private TextMeshProUGUI mainSliderText;
    [SerializeField] private TextMeshProUGUI musicSliderText;
    [SerializeField] private TextMeshProUGUI SFXSliderText;

    [Header("Value References")]
    [SerializeField] private string masterVolumeString;
    [SerializeField] private string musicVolumeString;
    [SerializeField] private string SFXVolumeString;
    [Space]
    [SerializeField] private string toggleString;

    [Header("Values")]
    [SerializeField] private float previousMasterVolume = 0.5f;              // Para recordar el volumen antes de silenciar
    [SerializeField] private float previousMusicVolume = 0.5f;               // Para recordar el volumen de música
    [SerializeField] private float previousSFXVolume = 0.5f;                 // Para recordar el volumen de efectos 


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        // Iniciar los valores
        StartCoroutine(InitialDelay());
    }

    // Necesario por fallo de orden de carga de Unity
    private IEnumerator InitialDelay()
    {
        yield return null;

        SetInitialValues();
    }

    private void SetInitialValues()
    {
        // Comprobación inicial
        if (audioMixer == null)
        {
            Debug.LogError("Audio Mixer not found. Might be missing a reference or simply not exist.");
            return;
        }

        // Silencio
        bool isSilent = PlayerPrefs.GetInt(toggleString, 0) == 1;
        if (toggleSilenciar != null)       
            toggleSilenciar.SetIsOnWithoutNotify(isSilent);
                   

        // Imagen
        toggleImage.sprite = isSilent ? mutedImage : unmutedImage;
        if (soundImage) soundImage.sprite = toggleImage.sprite;


        // Master Audio
        if (mainSlider != null)
        {
            float savedVolume = PlayerPrefs.GetFloat(masterVolumeString, 0.5f);
            previousMasterVolume = savedVolume;

            if (!isSilent)           
                mainSlider.value = savedVolume;                         
            else
                mainSlider.value = mainSlider.minValue;

            SetAudio(mainSlider, masterVolumeString, mainSliderText);
        }

        // Music Audio
        if (musicSlider != null)
        {
            float savedVolume = PlayerPrefs.GetFloat(musicVolumeString, 0.5f);
            previousMusicVolume = savedVolume;

            if (!isSilent)
                musicSlider.value = savedVolume;
            else
                musicSlider.value = musicSlider.minValue;

            SetAudio(musicSlider, musicVolumeString, musicSliderText);
        }

        // SFX Audio
        if (SFXSlider != null)
        {
            float savedVolume = PlayerPrefs.GetFloat(SFXVolumeString, 0.5f);
            previousSFXVolume = savedVolume;

            if (!isSilent)
                SFXSlider.value = savedVolume;
            else
                SFXSlider.value = SFXSlider.minValue;

            SetAudio(SFXSlider, SFXVolumeString, SFXSliderText);
        }
    }

    #region Sliders
    // Entrada de inputs del jugador. Llamado desde todos los sliders de audio.
    public void SliderSwitchCase(string sliderName)
    {
        switch (sliderName.ToLower().Trim())
        {
            case "master":
                SetAudio(mainSlider, masterVolumeString, mainSliderText);
                break;

            case "music":
                SetAudio(musicSlider, musicVolumeString, musicSliderText);
                break;

            case "sfx":
                SetAudio(SFXSlider, SFXVolumeString, SFXSliderText);
                break;

            default:
                Debug.LogError("No se ha proporcionado un valor correcto.");
                break;
        }
    }

    // Setea el slider y el AudioMixer
    private void SetAudio(Slider audioSlider, string audioReference, TextMeshProUGUI valueText)
    {
        if (audioSlider == null)
            return;

        float volume = audioSlider.value;

        // Ajustar volumen en el AudioMixer
        if (audioMixer != null)
            audioMixer.SetFloat(audioReference, Mathf.Log10(volume) * 20);

        // Texto
        float volumeText = volume * 100;
        valueText.text = volumeText.ToString("0");

        // Guardar preferencia
        PlayerPrefs.SetFloat(audioReference, volume);
        PlayerPrefs.Save();
    }
    #endregion

    #region Mute
    // Silencia o restaura el volumen original
    public void MuteToggle(bool isSilent)
    {
        // Imagen
        toggleImage.sprite = isSilent ? mutedImage : unmutedImage;
        if (soundImage) soundImage.sprite = toggleImage.sprite;

        if (isSilent)
        {
            // Guarda valores, silencia y actualiza sliders
            MuteAudio(mainSlider, masterVolumeString, ref previousMasterVolume);
            MuteAudio(musicSlider, masterVolumeString, ref previousMusicVolume);
            MuteAudio(SFXSlider, masterVolumeString, ref previousSFXVolume);
        }
        else
        {
            // Restaura valores y actualiza sliders
            UnMuteAudio(mainSlider, masterVolumeString, previousMasterVolume);
            UnMuteAudio(musicSlider, musicVolumeString, previousMusicVolume);
            UnMuteAudio(SFXSlider, SFXVolumeString, previousSFXVolume);
        }

        // Guardar preferencia
        PlayerPrefs.SetInt(toggleString, isSilent ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void MuteAudio(Slider audioSlider, string volumeReference, ref float previousVolume)
    {
        // Guardar valores
        if (audioSlider != null && audioSlider.value > audioSlider.minValue)
            previousVolume = audioSlider.value;

        // Silenciar
        audioMixer.SetFloat(volumeReference, -80f);

        // Actualizar sliders
        if (audioSlider != null) audioSlider.value = audioSlider.minValue;
    }

    private void UnMuteAudio(Slider audioSlider, string volumeReference, float previousVolume)
    {
        // Restaurar valores
        audioMixer.SetFloat(volumeReference, Mathf.Log10(previousVolume) * 20);

        // Actualizar sliders
        if (audioSlider != null) audioSlider.value = previousVolume;
    }
    #endregion
}