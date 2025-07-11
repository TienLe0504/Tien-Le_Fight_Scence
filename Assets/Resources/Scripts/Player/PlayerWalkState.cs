using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerGroundState
{
    public PlayerWalkState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName){}

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        player.isAttack = false;
        if (player.isIdle && !player.isGrounded)
        {
            stateMachine.ChangeState(player.playerModel.idleState);
            return; 
        }
        player.ApplyMovementForce(player.playerModel.moveDirection.normalized * player.playerModel.speed, player.playerModel.moveDirection);
    }
}
