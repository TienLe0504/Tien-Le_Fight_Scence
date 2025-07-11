using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInjuryState : PlayerState
{
    public PlayerInjuryState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();
        player.ApplyMovementForce(Vector3.zero, Vector3.zero);
        player.anim.SetFloat(CONST.VAR_FLOAT_COUNT_KEY, player.playerModel.injuryComboCount);

    }

    public override void Exit()
    {
        base.Exit();
        player.wasHit = false;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();
        if (player.wasHit)
        {
            player.wasHit = false;
            stateMachine.ChangeState(player.playerModel.idleState);
        }
    }
}
