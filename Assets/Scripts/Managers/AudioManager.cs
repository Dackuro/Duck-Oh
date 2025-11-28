using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;


    [SerializeField] private AudioSource soundFXObject;

    public List<GameObject> loopingSFXObjects;


    private void Awake()
    {
        // Singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        loopingSFXObjects = new List<GameObject>();
    }


    // Sonido
    public void PlaySFXClip(AudioClip audioClip, string audioName, Transform spawnTransform, float volume, bool isLooped = false, bool fadeIn = false)
    {
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = audioClip;   // Clip
        audioSource.name = audioName;   // Nombre
        audioSource.volume = volume;    // Volumen
        audioSource.loop = isLooped;    // Loop

        if (!isLooped)
        {
            float clipLength = audioSource.clip.length;

            audioSource.Play();

            if (fadeIn)
            {
                clipLength -= 0.5f;

                StartCoroutine(FadeAudio(audioSource, 0f, volume, 0.6f, false, clipLength, isLooped));  // Fade-In
                StartCoroutine(FadeAudio(audioSource, volume, 0f, 0.6f, true, clipLength, isLooped));   // Fade-Out

                return;
            }

            Destroy(audioSource.gameObject, clipLength);
        }
        else
        {
            // Randomiza cuándo empieza el audio para evitar repetición
            audioSource.time = Random.Range(0f, audioClip.length);
            audioSource.Play();

            if (fadeIn)
                StartCoroutine(FadeAudio(audioSource, 0f, volume, 0.6f, false, 0f, isLooped));  // Fade-In

            loopingSFXObjects.Add(audioSource.gameObject);
        }
    }

    // Sonido random dentro de un array de sonidos
    public void PlayRandomSFXClip(List<AudioClip> audioClips, string audioName, Transform spawnTransform, float volume, bool isLooped = false, bool fadeIn = false)
    {
        int randomClip = Random.Range(0, audioClips.Count);

        AudioClip audioClipSelected = audioClips[randomClip];

        PlaySFXClip(audioClipSelected, audioName, spawnTransform, volume, isLooped, fadeIn);
    }

    // Destruye el objeto exacto que se pida por nombre
    public void DestroyLoopedClip(string loopedName, bool fadeIn)
    {
        for (int i = loopingSFXObjects.Count - 1; i >= 0; i--)
        {
            GameObject sfxObject = loopingSFXObjects[i];

            if (sfxObject != null && sfxObject.name == loopedName)
            {
                if (fadeIn)
                {
                    AudioSource audioSource = sfxObject.GetComponent<AudioSource>();

                    StartCoroutine(FadeAudio(audioSource, audioSource.volume, 0f, 0.8f, true, 0f, true));   // Fade-Out
                }
                else
                {
                    Destroy(sfxObject.gameObject);
                }

                loopingSFXObjects.RemoveAt(i);
            }
        }
    }

    // Fade-In y Fade-Out, dependiente de la variable booleana "fadeOut"
    private IEnumerator FadeAudio(AudioSource audioSource, float startVolume, float endVolume, float duration, bool fadeOut, float delay, bool isLoop)
    {
        if (fadeOut)
            yield return new WaitForSeconds(delay);

        float currentTime = 0f;

        audioSource.volume = startVolume;

        // Timer
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, endVolume, currentTime / duration);
            yield return null;
        }

        audioSource.volume = endVolume;

        if (fadeOut)
            Destroy(audioSource.gameObject);
    }
}