using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateCounterAttack : PlayerState
{
    public PlayerStateCounterAttack(Player _player, PlayerStateMachine _stateMachine, string _anim) : base(_player, _stateMachine, _anim)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTime = player.counterAttackDuration;
        player.animator.SetBool("SuccessCounterAttack", false);
    }

    public override void Update()
    {
        base.Update();

        if (stateTime <= 0 || stopAnimations) 
        {
            stateMachine.ChangeState(player.stateIdle);
            return;
        }
        
        Collider2D[] hits = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);
        foreach (var hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy)
            {
                if (enemy.canBeStunned)
                {
                    enemy.Stun();
                    player.animator.SetBool("SuccessCounterAttack", true);
                }
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        
        player.animator.SetBool("SuccessCounterAttack", false);
    }
}
