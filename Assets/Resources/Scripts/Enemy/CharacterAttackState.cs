using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttackState : CharacterState
{
    public float timer = 0f;
    public CharacterAttackState(Character _enemy, CharacterStateMachine _stateMachine, string _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        timer = 0f;
        character.isTriggerAttackEnemy = false;
    }

    public override void Exit()
    {
        base.Exit();
        character.isTriggerAttackEnemy = false;
        character.isAttack = false;
        character.didHitTarget = false;
        character.characterModel.attackTimer = 0f;
        character.characterModel.IncreaseAttackCount(CONST.ATTACK_COMBO_WINDOW_CHARACTER);

    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        character.anim.SetFloat(CONST.VAR_FLOAT_COUNT_KEY, character.characterModel.attackComboCount);
        base.Update();
        if (character.isTriggerAttackEnemy)
        {
            character.isAttackCoolingDown = true;
            character.isAttack = false;
            character.isTriggerAttackEnemy = false;
            character.characterModel.attackTimer = 0f;
            stateMachine.ChangeState(character.characterModel.idleState);
            return;
        }
        if(character.isInjured)
        {
            stateMachine.ChangeState(character.characterModel.injuryState);
            return;
        }
        if (character.didHitTarget)
        {
            character.didHitTarget = false;
            character.PerformMeleeAttack(character.whatIsLayer, character.tagObject.ToString());
        }
        if (character.isContinueAttack)
        {
            timer += Time.deltaTime;
            if (timer>=character.characterModel.attackComboWindow)
            {
                character.isAttack = character.CheckForEntityInRange(character.whatIsLayer, character.tagObject.ToString());
                if(!character.isAttack)
                {
                    character.isAttackCoolingDown = true;
                    character.isTriggerAttackEnemy = false;
                    stateMachine.ChangeState(character.characterModel.walkState);
                    return;
                }
            }
        }
    }
}
