using UnityEngine;

public class PushedSubState : PlayerState
{
    public override string Name => "Pushed State";
    public PushedSubState(StateMachine STATEMACHINE, PlayerGeneral PLAYER, Vector3 dir = default) : base(STATEMACHINE, PLAYER) 
    {
        pushDirection = dir;
    }

    Vector3 pushDirection;
    private float pushDuration;
    private float stateDuration;
    private float timer = 0f;
    private Rigidbody rb;
    private bool hasPushed;


    public override void Enter()
    {
        base.Enter();


        // Feedback
        AudioManager.instance.PlaySFXClip(PLAYER.FEEDBACK.pushedAudio, "Pushed AudioSource", PLAYER.transform, 0.5f);
        ParticlesManager.instance.PlayParticleSystem(PLAYER.FEEDBACK.pushedParticles, "Pushed ParticleSystem", PLAYER.transform.position + Vector3.up * 0.5f, PLAYER.transform.rotation);


        rb = PLAYER.Rigidbody;

        PLAYER.CONFIGURATION.isPushed = true;
        PLAYER.INPUTTRANSFORMER.EnableInputs(false);

        pushDuration = PLAYER.CONFIGURATION.PUSHDURATION * PLAYER.CONFIGURATION.PUSHFORCE;
        stateDuration = PLAYER.CONFIGURATION.PUSHEDSTATEDURATION * PLAYER.CONFIGURATION.PUSHFORCE;

        timer = 0f;
        hasPushed = false;    
    }

    public override void Update()
    {
        base.Update();

        timer += Time.deltaTime;

        if (timer >= stateDuration)
        {
            PLAYER.CONFIGURATION.isPushed = false;
            PLAYER.INPUTTRANSFORMER.EnableInputs(true);
            STATEMACHINE.ChangeState(PLAYER.STATES.OnAirState(STATEMACHINE, false));
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (rb == null) 
            return;

        if (!hasPushed)
        {
            rb.angularVelocity = Vector3.zero;
            PLAYER.MOVEMENT.StartPush(pushDirection);

            hasPushed = true;
        }
        else if (timer < pushDuration)
        {
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, Time.fixedDeltaTime);
        }
        else
        {
            rb.linearVelocity *= 0.98f;
        }
    }
}
