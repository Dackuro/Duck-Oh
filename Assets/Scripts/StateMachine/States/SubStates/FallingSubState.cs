using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class FallingSubState : PlayerState
{
    public override string Name => "Falling SubState";
    public FallingSubState(StateMachine STATEMACHINE, PlayerGeneral PLAYER) : base(STATEMACHINE, PLAYER) { }

    public override void Update()
    {
        if (PLAYER.COLLISION.GROUND)
            STATEMACHINE.ChangeState(PLAYER.STATES.IdleSubState(STATEMACHINE));
    }
}
