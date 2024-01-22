using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateGrounded : PlayerState
{
    public PlayerStateGrounded(Player _player, PlayerStateMachine _stateMachine, string _anim) : base(_player, _stateMachine, _anim)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (player.input.actions["attack"].triggered && player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.StateAttack);
            return;
        }
        
        if (player.input.actions["jump"].triggered && player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.stateJump);
            return;
        }

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
