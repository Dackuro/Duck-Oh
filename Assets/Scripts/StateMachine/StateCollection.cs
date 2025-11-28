using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateCollection
{
    private PlayerGeneral player;
    public StateCollection(PlayerGeneral player) => this.player = player;



    // States
    public OnGroundState OnGroundState(StateMachine sm) => new OnGroundState(sm, player);
    public OnAirState OnAirState(StateMachine sm, bool isJumping) => new OnAirState(sm, player, isJumping);

    // SubStates
    public IdleSubState IdleSubState(StateMachine sm) => new IdleSubState(sm, player);
    public MoveSubState MoveSubState(StateMachine sm) => new MoveSubState(sm, player);
    public JumpingSubState JumpingSubState(StateMachine sm) => new JumpingSubState(sm, player);
    public FallingSubState FallingSubState(StateMachine sm) => new FallingSubState(sm, player);
    public DashSubState DashSubState(StateMachine sm) => new DashSubState(sm, player);
    public StompSubState StompSubState(StateMachine sm) => new StompSubState(sm, player);
    public PushedSubState PushedSubState(StateMachine sm, Vector3 direction) => new PushedSubState(sm, player, direction);
}
