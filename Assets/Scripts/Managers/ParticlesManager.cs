using System.Collections.Generic;
using UnityEngine;

public class ParticlesManager : MonoBehaviour
{
    public static ParticlesManager instance;


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
    }

    public void PlayParticleSystem(ParticleSystem particles, string particleName, Vector3 spawnPosition, Quaternion spawnRotation, bool isLooped = false)
    {
        ParticleSystem newParticles = Instantiate(particles, spawnPosition, spawnRotation);

        newParticles.name = particleName;

        newParticles.Play();

        var main = newParticles.main;

        // Destroy after play
        if (!main.loop)
            Destroy(newParticles.gameObject, main.duration);
    }
}
