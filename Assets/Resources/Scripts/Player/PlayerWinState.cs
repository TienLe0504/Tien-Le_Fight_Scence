using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerWinState : PlayerState
{
    public PlayerWinState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        player.isWinner = false;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();
        if (player.isWinner)
        {
            player.isWinner = false;
            GameManger.Instance.isPlayerGame = false;
            GameManger.Instance.WinGame();
            stateMachine.ChangeState(player.playerModel.idleState);
        }
    }
}
