using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateAir : PlayerState
{
    public PlayerStateAir(Player _player, PlayerStateMachine _stateMachine, string _anim) : base(_player, _stateMachine, _anim)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        player.animator.SetFloat("yVelocity", player.rb.velocity.y);
        
        if (player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.stateIdle);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
