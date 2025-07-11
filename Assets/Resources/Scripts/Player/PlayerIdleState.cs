using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(Player player, PlayerStateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName){}

    public override void Enter()
    {
        base.Enter();
        player.ApplyMovementForce(Vector3.zero, Vector3.zero);

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (!player.isIdle && !player.isGrounded)
        {
            stateMachine.ChangeState(player.playerModel.walkState);
        }

    }
}
