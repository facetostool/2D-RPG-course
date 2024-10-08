using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateDash : PlayerState
{
    public PlayerStateDash(Player _player, PlayerStateMachine _stateMachine, string _anim) : base(_player, _stateMachine, _anim)
    {
    }
    
    private float imageFXTimer;

    public override void Enter()
    {
        base.Enter();

        stateTime = player.skills.dash.dashTime;
        player.skills.dash.Use();
        player.skills.dash.CreateCloneOnStart();
        SoundManager.instance.PlaySFX(SfxEffect.Dash);
    }

    public override void Update()
    {
        base.Update();
        
        if (imageFXTimer > 0)
        {
            imageFXTimer -= Time.deltaTime;
        } else {
            player.fx.ImageFX(player.sr.sprite);
            imageFXTimer = player.fx.imageFrequency;
        }

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
        
        float dashDir = player.faceDir;
        if (player.moveVector.x != 0)
        {
            dashDir = player.moveVector.x;
        }
        
        player.rb.velocity = new Vector2(player.skills.dash.dashSpeed*dashDir, 0);
    }

    public override void Exit()
    {
        base.Exit();
        
        player.skills.dash.CreateCloneOnEnd();
    }
}
