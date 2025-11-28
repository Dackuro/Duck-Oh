using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class OnAirState : PlayerState
{
    private StateMachine lowLevelMachine;
    public StateMachine subMachine => lowLevelMachine;

    public override string Name => "OnAir State";

    [Header("Before looking for ground again")]
    float timer = 0;
    float duration = 0.1f;
    bool canDetectGround = false;
    bool isJumping;


    public OnAirState(StateMachine STATEMACHINE, PlayerGeneral PLAYER, bool isJumping = false) : base(STATEMACHINE, PLAYER)
    {
        lowLevelMachine = new StateMachine();
        this.isJumping = isJumping;
    }

    public override void Enter()
    {
        if (isJumping)
            lowLevelMachine.ChangeState(PLAYER.STATES.JumpingSubState(lowLevelMachine));
        else         
            lowLevelMachine.ChangeState(PLAYER.STATES.FallingSubState(lowLevelMachine));

        PLAYER.ANIMATION.SetAnimation(PlayerAnimation.Wings);
    }

    public override void Update()
    {
        lowLevelMachine.Update();

        timer += Time.deltaTime;
        if (timer >= duration)        
            canDetectGround = true;
        
        // Ground
        if (PLAYER.COLLISION.GROUND && canDetectGround)
            STATEMACHINE.ChangeState(PLAYER.STATES.OnGroundState(STATEMACHINE));

        // Jump
        if (!PLAYER.COLLISION.GROUND && PLAYER.INPUTTRANSFORMER.INPUTJUMP > Time.time && PLAYER.CONFIGURATION.remainingJumps > 0)
        {
            PLAYER.INPUTTRANSFORMER.ProcessInputJump(0);
            PLAYER.CONFIGURATION.remainingJumps--;

            STATEMACHINE.ChangeState(PLAYER.STATES.OnAirState(STATEMACHINE, true));
        }

        // Dash
        if (PLAYER.INPUTTRANSFORMER.INPUTDASH > Time.time && Time.time >= PLAYER.CONFIGURATION.lastDashTime + PLAYER.CONFIGURATION.DASHCOOLDOWN)
        {
            PLAYER.INPUTTRANSFORMER.ProcessInputDash(0);
            lowLevelMachine.ChangeState(PLAYER.STATES.DashSubState(lowLevelMachine));
        }

        // Stomp
        if (PLAYER.INPUTTRANSFORMER.INPUTSTOMP > Time.time && !PLAYER.CONFIGURATION.isStomping)
        {
            lowLevelMachine.ChangeState(PLAYER.STATES.StompSubState(lowLevelMachine));
        }

        // Push
        if (PLAYER.CONFIGURATION.isPushed && !(lowLevelMachine.state is PushedSubState))
        {
            STATEMACHINE.ChangeState(PLAYER.STATES.PushedSubState(STATEMACHINE, PLAYER.CONFIGURATION.pusherPosition));
        }

        // Quack
        if (PLAYER.INPUTTRANSFORMER.INPUTQUACK > Time.time && PLAYER.CONFIGURATION.canQuack)
        {
            PLAYER.CONFIGURATION.Quack();

            if (PLAYER.FEEDBACK.quackAudios.Count > 0)
                AudioManager.instance.PlayRandomSFXClip(PLAYER.FEEDBACK.quackAudios, "Quack AudioSource", PLAYER.transform, 0.5f);
            if (PLAYER.FEEDBACK.quackParticles)
                ParticlesManager.instance.PlayParticleSystem(PLAYER.FEEDBACK.quackParticles, "Quack ParticleSystem", PLAYER.transform.position, PLAYER.transform.rotation);         
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        lowLevelMachine.FixedUpdate();

        if (!PLAYER.CONFIGURATION.isDashing)
            PLAYER.MOVEMENT.VelocityMovementInAir(PLAYER.INPUTTRANSFORMER.INPUTVECTORNORMAL, PLAYER.mainCamera);
    }
    public override void LateUpdate() => lowLevelMachine.LateUpdate();
}