using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using static Cinemachine.CinemachineTriggerAction.ActionSettings;

public class CharacterModel : Model
{
    public float timeToIdle;
    public float timeToMove;
    public float attackCoolDown;

    public CharacterStateMachine stateMachine { get; private set; }
    public CharacterIdleState idleState { get; private set; }
    public CharacterWalkState walkState { get; private set; }
    public CharacterAttackState attackState { get; private set; }
    public CharacterInjuryState injuryState { get; private set; }
    public CharacterKnockOutState knockOutState { get; private set; }
    public CharacterWinState winState { get; private set; }
    public CharacterModel(Character enemy, float health, float damage, float attackCoolDownTime, float timeToMove,float timeToIdle, float timeAttackContinue)
    {
        this.speed = CONST.SPEED_CHARACTER;
        this.rotationSpeed = CONST.ROTATION_SPEED_CHARACTER;
        this.attackCooldown = attackCoolDownTime;
        this.health = health;
        this.damage = damage;   
        this.timeToMove = timeToMove;
        this.timeToIdle = timeToIdle;
        this.attackComboWindow = timeAttackContinue;
        maxHealth = health;
        stateMachine = new CharacterStateMachine();
        idleState = new CharacterIdleState(enemy, stateMachine, CONST.IDLE);
        walkState = new CharacterWalkState(enemy, stateMachine, CONST.WALK);
        attackState = new CharacterAttackState(enemy, stateMachine, CONST.ATTACK);
        injuryState = new CharacterInjuryState(enemy, stateMachine, CONST.INJURY);
        knockOutState = new CharacterKnockOutState(enemy, stateMachine, CONST.KNOCK_OUT);
        winState = new CharacterWinState(enemy, stateMachine, CONST.WIN);
        stateMachine.Initialize(idleState);
    }
}
