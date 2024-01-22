using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateDash : PlayerState
{
    public PlayerStateDash(Player _player, PlayerStateMachine _stateMachine, string _anim) : base(_player, _stateMachine, _anim)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTime = player.dashTime;
    }

    public override void Update()
    {
        base.Update();

        if (player.IsWallDetected())
        {
            stateMachine.ChangeState(player.stateWallSlide);
            return;
        }
        
        if (stateTime <= 0)
        {
            player.SetVelocity(0, 0);
            stateMachine.ChangeState(player.stateIdle);
            return;
        }
        
        player.rb.velocity = new Vector2(player.dashSpeed*player.faceDir, 0);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
