using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class OnGroundState : PlayerState
{
    private StateMachine lowLevelMachine;
    public StateMachine subMachine => lowLevelMachine;

    public override string Name => "OnGround State";
    public OnGroundState(StateMachine STATEMACHINE, PlayerGeneral PLAYER) : base(STATEMACHINE, PLAYER)
    {
        lowLevelMachine = new StateMachine();
    }

    public override void Enter()
    {
        lowLevelMachine.ChangeState(PLAYER.STATES.IdleSubState(lowLevelMachine));

        PLAYER.CONFIGURATION.ResetJumps();
        PLAYER.CONFIGURATION.ResetStomp();
    }

    public override void Update()
    {
        lowLevelMachine.Update();

        base.Update();

        // Jump
        if (PLAYER.COLLISION.GROUND && PLAYER.INPUTTRANSFORMER.INPUTJUMP > Time.time && !(lowLevelMachine.state is PushedSubState))
        {
            PLAYER.INPUTTRANSFORMER.ProcessInputJump(0);
            PLAYER.CONFIGURATION.remainingJumps--;

            STATEMACHINE.ChangeState(PLAYER.STATES.OnAirState(STATEMACHINE, true));
        }

        // Fall
        if (!PLAYER.COLLISION.GROUND)
        {
            // Solo un salto en el aire
            PLAYER.CONFIGURATION.remainingJumps--;

            STATEMACHINE.ChangeState(PLAYER.STATES.OnAirState(STATEMACHINE, false));
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
    
    public override void FixedUpdate() => lowLevelMachine.FixedUpdate();
    public override void LateUpdate() => lowLevelMachine.LateUpdate();
}
