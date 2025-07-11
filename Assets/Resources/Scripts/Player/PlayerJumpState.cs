using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{

    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        player.hasTriggeredEvent = false;
        player.isJumping = false;
        player.isGrounded = false;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();
        if (player.isInjured)
        {
            player.isInjured = false;
            stateMachine.ChangeState(player.playerModel.injuryState);
            return;
        }
        if (player.hasTriggeredEvent)
        {
            stateMachine.ChangeState(player.playerModel.idleState);
            player.isJumping = false;
            player.isGrounded = false;
            player.hasTriggeredEvent = false;
        }
    }
}
