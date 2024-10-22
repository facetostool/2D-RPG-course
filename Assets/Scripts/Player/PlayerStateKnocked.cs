using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateKnocked : PlayerState
{
    public PlayerStateKnocked(Player _player, PlayerStateMachine _stateMachine, string _anim) : base(_player, _stateMachine, _anim)
    {
    }

    public override void Enter()
    {
        base.Enter();
        SoundManager.instance.PlaySFX(SfxEffect.WomenSigh1);
        player.MakeBusyFor(2*player.knockedDuration);
        stateTime = player.knockedDuration;
    }

    public override void Update()
    {
        base.Update();
        
        player.rb.velocity = new Vector2(player.knockedForce.x * player.knockedDirection, player.knockedForce.y);
        player.animator.SetFloat("yVelocity", player.rb.velocity.y);
        if (stateTime <= 0)
        {
            stateMachine.ChangeState(player.stateIdle);
            return;
        }
        
    }

    public override void Exit()
    {
        base.Exit();
    }
}
