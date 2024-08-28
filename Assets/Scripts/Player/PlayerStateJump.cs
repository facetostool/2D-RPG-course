using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateJump : PlayerState
{
    public PlayerStateJump(Player _player, PlayerStateMachine _stateMachine, string _anim) : base(_player, _stateMachine, _anim)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.rb.velocity = new Vector2(player.rb.velocity.x, player.jumpForce);
        
        SoundManager.instance.PlaySFX((int)SfxEffect.PlayerJump);
    }

    public override void Update()
    {
        base.Update();

        if (!player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.stateAir);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
