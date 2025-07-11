using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWalkState : CharacterGroundState
{
    public CharacterWalkState(Character _enemy, CharacterStateMachine _stateMachine, string _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        character.timerAttack = 0f;
        character.timeSinceLastSwitch = 0f;
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
        if (character.isFlip)
            return;
        character.timeSinceLastSwitch += Time.deltaTime;

        bool isAttack = character.CheckForEntityInRange(character.whatIsLayer, character.tagObject.ToString());

        if (isAttack && character.isAttackCoolingDown && character.timeSinceLastSwitch >= character.stateSwitchDelay)
        {
            character.isWaitToMove = true;
            stateMachine.ChangeState(character.characterModel.idleState);
        }
        else
        {

          
                if (character.canAttackContinuously)
                {
                    character.timerAttack += Time.deltaTime;
                    if (character.timerAttack >= character.characterModel.timeToMove)
                    {
                        character.isDelayAttack = true;
                        character.timerAttack = 0f;
                        stateMachine.ChangeState(character.characterModel.idleState);
                        return;
                    }
                }
                Entity item = GameManger.Instance.GetPosPosition(character.tagObject, character.transform.position);
                if(item == null)
                {
                    if(character.tagObject == EntityType.Player)
                    {
                        if (!GameManger.Instance.playerCurrent.isAlive)
                        {
                            stateMachine.ChangeState(character.characterModel.winState);
                            return;
                        }
                    }
                    if (!GameManger.Instance.playerCurrent.isAlive)
                    {
                        stateMachine.ChangeState(character.characterModel.winState);
                        return;
                    }
                    if (character.tagObject == EntityType.Enemy)
                    {
                        stateMachine.ChangeState(character.characterModel.winState);
                        return;
                    }
                    item = GameManger.Instance.playerCurrent;

                }
                Vector3 posPlayer = item.transform.position;
                Vector3 posEnemy = character.transform.position;
                Vector3 directionToPlayer = (posPlayer - posEnemy).normalized;
                character.characterModel.moveDirection = new Vector3(directionToPlayer.x, 0f, directionToPlayer.z);
                character.ApplyMovementForce(directionToPlayer * character.characterModel.speed, directionToPlayer);

            

        }
    }
}
