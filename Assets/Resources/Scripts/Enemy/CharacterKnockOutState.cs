using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterKnockOutState : CharacterState
{
    public CharacterKnockOutState(Character _character, CharacterStateMachine _stateMachine, string _animBoolName) : base(_character, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        character.isAlive = false;
        character.capsuleCollider.isTrigger = true;
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
        if (character.isKnockOut)
        {
            character.isKnockOut = false;
            if(character.tagObject == EntityType.Enemy && character.hasCamera)
            {
                character.hasCamera = false;
                GameManger.Instance.ChangeCamera();
            }
            character.gameObject.SetActive(false);
        }
    }
}
