using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateAttack : PlayerState
{
    private int attackCounter = 0;
    
    public PlayerStateAttack(Player _player, PlayerStateMachine _stateMachine, string _anim) : base(_player, _stateMachine, _anim)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        attackCounter++;
        player.animator.SetInteger("AttackCounter", attackCounter);
        if (attackCounter >= 3)
        {
            attackCounter = 0;
        }
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(0,0);
        
        if (stopAnimations)
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
