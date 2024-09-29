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
            SoundManager.instance.PlaySFX(SfxEffect.Landing);
            stateMachine.ChangeState(player.stateIdle);
            return;
        }

        if (player.IsWallDetected())
        {
            // check if player's x velocity is faced to the wall
            if (player.moveVector.x > 0 && player.faceDir > 0 || player.moveVector.x < 0 && player.faceDir < 0)
            {
                stateMachine.ChangeState(player.stateWallSlide);
                return;
            }
        }
        
        player.rb.velocity = new Vector2(player.moveVector.x * player.moveSpeed * 0.8f, player.rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
