using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundState : PlayerState
{
    public PlayerGroundState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
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
        else
        {
            if (GameManger.Instance.IsWinBattle())
            {
                stateMachine.ChangeState(player.playerModel.winState);
            }
            if (player.isAttackCoolingDown)
            {
                player.playerModel.attackTimer += Time.deltaTime;
                if (player.playerModel.attackTimer >= player.playerModel.attackCooldown)
                {
                    player.isAttackCoolingDown = false;
                }
            }
            if (player.isJumping)
            {
                stateMachine.ChangeState(player.playerModel.jumpState);
            }
            if(player.isAttack && player.isIdle && !player.isGrounded && !player.isAttackCoolingDown)
            {
                stateMachine.ChangeState(player.playerModel.attackState);
            }

        }
    }
}
