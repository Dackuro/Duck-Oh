using UnityEngine;

public class IdleSubState : PlayerState
{
    public override string Name => "Idle SubState";

    public IdleSubState(StateMachine STATEMACHINE, PlayerGeneral PLAYER) : base(STATEMACHINE, PLAYER) { }

    public override void Enter()
    {
        base.Enter();
        PLAYER.MOVEMENT.VelocityIdle();
        PLAYER.ANIMATION.SetAnimation(PlayerAnimation.Idle);
    }

    public override void Update()
    {
        if (PLAYER.INPUTTRANSFORMER.INPUTVECTORNORMAL.magnitude > 0.1f)
        {
            if (PLAYER.INPUTTRANSFORMER.INPUTDASH > Time.time)
                PLAYER.INPUTTRANSFORMER.ProcessInputDash(0);

            STATEMACHINE.ChangeState(PLAYER.STATES.MoveSubState(STATEMACHINE));
        }
    }
}
