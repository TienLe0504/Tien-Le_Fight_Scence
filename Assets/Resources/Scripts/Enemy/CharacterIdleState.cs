using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterIdleState : CharacterGroundState
{
    public CharacterIdleState(Character _enemy, CharacterStateMachine _stateMachine, string _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        character.ApplyMovementForce(Vector3.zero, Vector3.zero);
        character.timerAttack = 0f;
        character.timeSinceLastSwitch = 0f;
    }

    public override void Exit()
    {
        base.Exit();
        character.isDelayAttack = false;


    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();

        character.timeSinceLastSwitch += Time.deltaTime;

        bool isAttack = character.CheckForEntityInRange(character.whatIsLayer,character.tagObject.ToString());

        if (!isAttack && character.timeSinceLastSwitch >= character.stateSwitchDelay)
        {
            if (character.canAttackContinuously)
            {
                if (character.isDelayAttack)
                {
                    character.timerAttack += Time.deltaTime;
                    if (character.timerAttack >= character.characterModel.timeToIdle)
                    {
                        character.isDelayAttack = false;
                        character.timerAttack = 0f;
                        ChangeWalkState();
                        return;
                    }
                }
                else
                {
                    ChangeWalkState();
                }
            }
            else
            {

                ChangeWalkState();
            }

        }

        
    }
    public void ChangeWalkState()
    {
        stateMachine.ChangeState(character.characterModel.walkState);
    }
}
