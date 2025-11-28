using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShockWaveGenerator : MonoBehaviour
{
    [Header("Start Animation")]
    public float time = 0f;

    public Vector3 startPos;
    public Vector3 endPos;

    [Header("ShockWave")]
    public GameObject shockWavePrefab;

    [Header("Intervals")]
    [Header("Values")]
    public float spawnInterval;
    [Space]
    // Base
    public float baseLowestValue;
    public float baseHighestValue;
    [Space]
    // Current
    public float lowestValue;
    public float highestValue;

    [Header("Speed")]
    public float baseSpeed;
    public float speed;

    [Header("Position")]
    public Vector3 spawnPosition;
    public Quaternion spawnRotation;
    public Quaternion particlesSpawnRotation;

    [Header("Feedback")]
    // Spawn
    [SerializeField] private AudioClip spawnClip;
    [Space]
    // ShockWave
    [SerializeField] private List<AudioClip> shockWaveClips;
    [SerializeField] private ParticleSystem shockWaveparticles;


    private void Awake()
    {
        // Animation
        endPos = transform.position;

        transform.position = startPos;

        // Generation Values
        lowestValue = baseLowestValue;
        highestValue = baseHighestValue;

        speed = baseSpeed;
    }
    private void Start()
    {
        // Animation
        StartCoroutine(SpeakerStart());

        // ShockWaves
        spawnPosition = new Vector3(transform.position.x, 0f, transform.position.z);
    }

    private IEnumerator SpeakerStart()
    {
        // Feedback
        if (spawnClip)
            AudioManager.instance.PlaySFXClip(spawnClip, "SoundWave AudioSource", transform, 0.5f, false, true);

        float elapsed = 0f;

        while (elapsed < time)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsed / time);

            elapsed += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(Generar());
    }

    private IEnumerator Generar()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            spawnInterval = Random.Range(lowestValue, highestValue);          

            GameObject shockWave = Instantiate(shockWavePrefab, spawnPosition, spawnRotation);

            ShockWave script = shockWave.GetComponent<ShockWave>();
            script.speed = speed;

            // Feedback
            if (shockWaveClips.Count > 0)
            {
                AudioManager.instance.PlayRandomSFXClip(shockWaveClips, "SoundWave AudioSource", shockWave.transform, 1f);
            }
                
            if (shockWaveparticles)
                ParticlesManager.instance.PlayParticleSystem(shockWaveparticles, "SoundWave ParticleSystem", shockWave.transform.position, particlesSpawnRotation);
        }
    }
}
