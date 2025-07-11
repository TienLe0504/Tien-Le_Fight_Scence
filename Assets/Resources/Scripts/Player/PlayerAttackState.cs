
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerState
{

    public PlayerAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.hasTriggeredEvent = false;
    }

    public override void Exit()
    {
        base.Exit();
        player.hasTriggeredEvent = false;
        player.isAttack= false;
        
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        player.anim.SetFloat(CONST.VAR_FLOAT_COUNT_KEY, player.playerModel.attackComboCount);
        base.Update();

        if (!player.isIdle)
        {
            player.playerModel.IncreaseAttackCount(CONST.ATTACK_COMBO_WINDOW_PLAYER);
            stateMachine.ChangeState(player.playerModel.walkState);
            return;
        }

        if (player.hasTriggeredEvent)
        {
            player.isAttackCoolingDown = true;
            player.playerModel.attackTimer = 0f;
            player.hasTriggeredEvent = false;
            player.isAttack = player.CheckForEntityInRange(player.whatIsLayer,player.tagObject.ToString());
            player.playerModel.IncreaseAttackCount(CONST.ATTACK_COMBO_WINDOW_PLAYER);
            stateMachine.ChangeState(player.playerModel.idleState);
            return;
        }
        if (player.didHitTarget)
        {
            player.didHitTarget = false;
            player.PerformMeleeAttack(player.whatIsLayer, player.tagObject.ToString());
        }
        if (player.isInjured)
        {
            player.isInjured = false;
            stateMachine.ChangeState(player.playerModel.injuryState);
            return;
        }
    }
}