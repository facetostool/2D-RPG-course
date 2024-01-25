using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateWallSlide : PlayerState
{
    public PlayerStateWallSlide(Player _player, PlayerStateMachine _stateMachine, string _anim) : base(_player, _stateMachine, _anim)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        
        if (player.IsGroundDetected() || !player.IsWallDetected())
        {
            stateMachine.ChangeState(player.stateIdle);
            return;
        }
        
        if (player.input.actions["jump"].triggered)
        {
            stateMachine.ChangeState(player.stateWallJump);
            return;
        }
        
        if (player.moveVector.x > 0 && player.faceDir < 0 || player.moveVector.x < 0 && player.faceDir > 0)
        {
            player.Flip();
            stateMachine.ChangeState(player.stateAir);
            return;
        }

        if (player.moveVector.y < 0)
        {
            player.rb.velocity = new Vector2(player.rb.velocity.x, player.rb.velocity.y);
        }
        else
        {
            player.rb.velocity = new Vector2(player.rb.velocity.x, 0.5f * player.rb.velocity.y);
        }


    }

    public override void Exit()
    {
        base.Exit();
    }
}
