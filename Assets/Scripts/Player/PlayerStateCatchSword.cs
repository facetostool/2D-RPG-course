using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateCatchSword : PlayerState
{
    public PlayerStateCatchSword(Player _player, PlayerStateMachine _stateMachine, string _anim) : base(_player, _stateMachine, _anim)
    {
    }

    public override void Enter()
    {
        base.Enter();
        SoundManager.instance.PlaySFX(SfxEffect.SwordThrow2);
        
        if (player.transform.position.x > player.sword.transform.position.x && player.faceDir == 1)
        {
            player.Flip();
        }
        else if (player.transform.position.x < player.sword.transform.position.x && player.faceDir == -1)
        {
            player.Flip();
        }
        
        player.SetVelocity(5*-player.faceDir, 0);
        stateTime = 0.1f;
        player.MakeBusyFor( stateTime);
    }

    public override void Update()
    {
        base.Update();

        if (stateTime <= 0)
        {
            player.SetVelocity(0,0);
        }
        
        if (finishedAnimation)
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
