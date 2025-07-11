using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : Model
{

    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerWalkState walkState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAttackState attackState { get; private set; }
    public PlayerInjuryState injuryState { get; private set; }
    public PlayerKnockOutState knockOutState { get; private set; }
    public PlayerWinState winState { get; private set; }
    public PlayerModel(Player player, float health, float damage)
    {
        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(player, stateMachine, CONST.IDLE);
        walkState = new PlayerWalkState(player, stateMachine, CONST.WALK);
        jumpState = new PlayerJumpState(player, stateMachine, CONST.JUMP);
        attackState = new PlayerAttackState(player, stateMachine, CONST.ATTACK);
        injuryState = new PlayerInjuryState(player, stateMachine, CONST.INJURY);
        knockOutState = new PlayerKnockOutState(player, stateMachine, CONST.KNOCK_OUT);
        winState = new PlayerWinState(player, stateMachine, CONST.WIN);
        this.health = health;
        this.damage = damage;
        stateMachine.Initialize(idleState);
    }

}
