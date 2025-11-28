using UnityEngine;

public class JumpingSubState : PlayerState
{
    public override string Name => "Jumping SubState";
    public JumpingSubState(StateMachine STATEMACHINE, PlayerGeneral PLAYER) : base(STATEMACHINE, PLAYER) { }


    public override void Enter()
    {
        base.Enter();

        PLAYER.CONFIGURATION.isDashing = false;

        // Modificador de la potencia del salto.                                            Primer Salto : Doble Salto
        float jumpMultiplier = PLAYER.CONFIGURATION.remainingJumps == PLAYER.CONFIGURATION.MAXJUMPS ? 1f : PLAYER.CONFIGURATION.JUMPMODIFIER;
        PLAYER.MOVEMENT.VelocityJump(jumpMultiplier);

        // Feedback
        AudioManager.instance.PlayRandomSFXClip(PLAYER.FEEDBACK.jumpAudios, "Jump AudioSource", PLAYER.transform, 0.5f);
        ParticlesManager.instance.PlayParticleSystem(PLAYER.FEEDBACK.jumpParticles, "Jump ParticleSystem", PLAYER.transform.position - Vector3.up * 0.5f, PLAYER.CONFIGURATION.HORIZONTAL);

        // Gravity
        Physics.gravity = Vector3.up * -80f;
    }

    public override void Update()
    {
        base.Update();

        if (PLAYER.MOVEMENT.VELOCITYY < 0)
        {
            STATEMACHINE.ChangeState(PLAYER.STATES.FallingSubState(STATEMACHINE));
        }
    }
}
