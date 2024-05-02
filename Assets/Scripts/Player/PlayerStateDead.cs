using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateDead : PlayerState
{
    public PlayerStateDead(Player _player, PlayerStateMachine _stateMachine, string _anim) : base(_player, _stateMachine, _anim)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        
        player.SetVelocity(0,0);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
