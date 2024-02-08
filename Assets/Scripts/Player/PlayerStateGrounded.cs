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
        
        if (Input.GetKeyDown(KeyCode.Mouse1) && player.IsGroundDetected() && !player.sword)
        {
            stateMachine.ChangeState(player.stateAimSword);
            return;
        }

        if (player.input.actions["CounterAttack"].triggered && player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.stateCounterAttack);
            return;
        }

        if (player.input.actions["Attack"].triggered && player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.stateAttack);
            return;
        }
        
        if (player.input.actions["Jump"].triggered && player.IsGroundDetected())
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
