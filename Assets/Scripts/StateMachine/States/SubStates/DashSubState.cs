using UnityEngine;

public class DashSubState : PlayerState
{
    public override string Name => "Dashing SubState";
    public DashSubState(StateMachine STATEMACHINE, PlayerGeneral PLAYER) : base(STATEMACHINE, PLAYER) { }

    private float dashTimer;

    public override void Enter()
    {
        base.Enter();

        PLAYER.CONFIGURATION.isDashing = true;
        dashTimer = 0f;
        PLAYER.CONFIGURATION.lastDashTime = Time.time;

        Vector3 dashDirection = new Vector3(PLAYER.INPUTTRANSFORMER.INPUTVECTORNORMAL.x, 0, PLAYER.INPUTTRANSFORMER.INPUTVECTORNORMAL.y);

        if (dashDirection.sqrMagnitude < 0.01f)
        {
            dashDirection = PLAYER.transform.forward;
        }

        // Feedback
        AudioManager.instance.PlayRandomSFXClip(PLAYER.FEEDBACK.dashAudios, "Dash AudioSource", PLAYER.transform, 0.5f);
        ParticlesManager.instance.PlayParticleSystem(PLAYER.FEEDBACK.dashParticles, "Dash ParticleSystem", PLAYER.transform.position - Vector3.forward * -1, PLAYER.CONFIGURATION.INVERTED);

        // StartDash
        PLAYER.MOVEMENT.StartDash(dashDirection.normalized);
    }

    public override void Update()
    {
        dashTimer += Time.deltaTime;

        if (dashTimer >= PLAYER.CONFIGURATION.DASHDURATION)
        {
            PLAYER.CONFIGURATION.isDashing = false;

            if (PLAYER.COLLISION.GROUND)
                STATEMACHINE.ChangeState(PLAYER.STATES.IdleSubState(STATEMACHINE));
            else
                STATEMACHINE.ChangeState(PLAYER.STATES.FallingSubState(STATEMACHINE));
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
