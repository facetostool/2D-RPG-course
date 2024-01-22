using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateWallJump : PlayerState
{
    public PlayerStateWallJump(Player _player, PlayerStateMachine _stateMachine, string _anim) : base(_player, _stateMachine, _anim)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTime = 0.1f;
        player.SetVelocity(-player.faceDir * player.wallJumpForce.x , player.wallJumpForce.y);
    }

    public override void Update()
    {
        base.Update();

        
        // player.SetVelocity(player.faceDir * player.wallJumpForce , player.wallJumpForce);
        
        if (stateTime <= 0)
        {
            stateMachine.ChangeState(player.stateAir);
            return;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
