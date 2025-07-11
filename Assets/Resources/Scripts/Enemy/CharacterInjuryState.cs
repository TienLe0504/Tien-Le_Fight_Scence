using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInjuryState : CharacterState
{
    public CharacterInjuryState(Character _enemy, CharacterStateMachine _stateMachine, string _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        character.ApplyMovementForce(Vector3.zero, Vector3.zero);
    }

    public override void Exit()
    {
        base.Exit();
        character.isInjured = false;
        character.wasHit = false;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        character.anim.SetFloat(CONST.VAR_FLOAT_COUNT_KEY, character.characterModel.injuryComboCount);
        base.Update();
        if (character.wasHit)
        {
            character.wasHit = false;
            stateMachine.ChangeState(character.characterModel.idleState);
        }
    }
}
