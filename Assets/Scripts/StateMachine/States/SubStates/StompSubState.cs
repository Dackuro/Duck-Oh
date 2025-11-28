using UnityEngine;

public class StompSubState : PlayerState
{
    public override string Name => "Stomping SubState";
    public StompSubState(StateMachine STATEMACHINE, PlayerGeneral PLAYER) : base(STATEMACHINE, PLAYER) { }

    private bool hasPlayedFeedback;

    public override void Enter()
    {
        base.Enter();

        hasPlayedFeedback = false;
        PLAYER.CONFIGURATION.isStomping = true;

        // Particles
        ParticlesManager.instance.PlayParticleSystem(PLAYER.FEEDBACK.startStompParticles, "StartStomp ParticleSystem", PLAYER.transform.position, Quaternion.Euler(90f, PLAYER.transform.rotation.eulerAngles.y, PLAYER.transform.rotation.eulerAngles.z));

        PLAYER.MOVEMENT.StartStomp();
    }

    public override void Update()
    {
        if (PLAYER.COLLISION.GROUND && !hasPlayedFeedback)
        {
            // Feedback
            AudioManager.instance.PlayRandomSFXClip(PLAYER.FEEDBACK.stompAudios, "Stomp AudioSource", PLAYER.transform, 0.7f);
            ParticlesManager.instance.PlayParticleSystem(PLAYER.FEEDBACK.stompParticles, "Stomp ParticleSystem", PLAYER.transform.position - Vector3.up * 0.5f, PLAYER.CONFIGURATION.HORIZONTAL);

            hasPlayedFeedback = true;
        }
    }
}
