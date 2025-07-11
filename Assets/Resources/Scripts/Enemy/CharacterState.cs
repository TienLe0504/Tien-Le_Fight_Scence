using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterState 
{
    protected Rigidbody rb;
    protected Character character;
    protected CharacterStateMachine stateMachine;

    private string animBoolName;
    public CharacterState(Character _enemy, CharacterStateMachine _stateMachine, string _animBoolName)
    {
        this.character = _enemy;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;
    }
    public virtual void Enter()
    {
        character.anim.SetBool(animBoolName, true);
        rb = character.rb;
    }

    public virtual void FixedUpdate()
    {
    }
    public virtual void Update() { }



    public virtual void Exit()
    {
        character.anim.SetBool(animBoolName, false);
    }

}
