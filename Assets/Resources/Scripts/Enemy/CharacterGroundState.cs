using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CharacterGroundState : CharacterState
{

    public CharacterGroundState(Character _enemy, CharacterStateMachine _stateMachine, string _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
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
        if(character.isInjured)
        {
            stateMachine.ChangeState(character.characterModel.injuryState);
            return;
        }
        else
        {
            if (character.isFlip)
            {

                Vector3 dir = character.opponentTranform.position - character.transform.position;
                dir.y = 0f;

                Quaternion rot = Quaternion.LookRotation(dir);
                Vector3 yOnly = new Vector3(0f, rot.eulerAngles.y, 0f);
                character.transform.DORotate(yOnly, 0.1f).OnComplete(() => {
                    character.isFlip = false;
                });
            }
            if (character.isAttack && !character.isAttackCoolingDown && !character.isFlip)
            {
                stateMachine.ChangeState(character.characterModel.attackState);
                return;
            }


        }


    }
}
