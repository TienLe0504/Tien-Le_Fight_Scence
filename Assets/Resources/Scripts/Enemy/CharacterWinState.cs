using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWinState : CharacterState
{
    public CharacterWinState(Character _enemy, CharacterStateMachine _stateMachine, string _animBoolName) : base(_enemy, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        character.isWinner = false;
        GameManger.Instance.isPlayerGame = false;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();
        if (character.isWinner)
        {
            character.isWinner = false;
            GameManger.Instance.isPlayerGame = false;
            if (character.tagObject == EntityType.Enemy)
            {
                GameManger.Instance.WinGame();
            }
            if(character.tagObject == EntityType.Player)
            {
                GameManger.Instance.LoseGame();
            }
            
        }
    }
}
