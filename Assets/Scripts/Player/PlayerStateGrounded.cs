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
        
        if (Input.GetKeyDown(KeyCode.Mouse1) && player.IsGroundDetected() && !player.sword && player.skills.throwSword.CanUse())
        {
            stateMachine.ChangeState(player.stateAimSword);
            return;
        }
        
        if (player.input.actions["Ultimate"].triggered && player.IsGroundDetected() && player.skills.blackHole.CanUse())
        {
            stateMachine.ChangeState(player.stateUltimate);
            return;
        }

        if (player.input.actions["CounterAttack"].triggered && player.IsGroundDetected() && player.skills.parry.CanUse())
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
