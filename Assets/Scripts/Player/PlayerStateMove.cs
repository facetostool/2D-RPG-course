using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMove : PlayerStateGrounded
{
    public PlayerStateMove(Player _player, PlayerStateMachine _stateMachine, string _anim): base(_player, _stateMachine, _anim)
    {
        
    }
    
    public override void Enter()
    {
        base.Enter();
    }
    
    public override void Update()
    {
        base.Update();

        if (player.moveVector.x == 0)
        {
            player.SetVelocity(0, player.rb.velocity.y);
            stateMachine.ChangeState(player.stateIdle);
            return;
        }

        player.SetVelocity(player.moveVector.x * player.moveSpeed, player.rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
