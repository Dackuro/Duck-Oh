using UnityEngine;
using System.Collections.Generic;

public class PlayerFeedback : MonoBehaviour
{
    [Header("General")]
    [Header("Quack")]
    public List<AudioClip> quackAudios;
    public ParticleSystem quackParticles;

    [Header("Feedback")]
    [Header("Jump")]
    public List<AudioClip> jumpAudios;
    public ParticleSystem jumpParticles;

    [Header("Dash")]
    public List<AudioClip> dashAudios;
    public ParticleSystem dashParticles;

    [Header("Stomp")]
    public List<AudioClip> stompAudios;
    public ParticleSystem stompParticles;
    public ParticleSystem startStompParticles;

    [Header("Push")]
    public AudioClip pushedAudio;
    public ParticleSystem pushedParticles;
}
