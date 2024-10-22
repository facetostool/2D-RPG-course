using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateCounterAttack : PlayerState
{
    private bool canCreateClone; // This is a new variable that will be used to check if the clone has already been created during this a single attack
    
    public PlayerStateCounterAttack(Player _player, PlayerStateMachine _stateMachine, string _anim) : base(_player, _stateMachine, _anim)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        canCreateClone = true;
        stateTime = player.counterAttackDuration;
        player.animator.SetBool("SuccessCounterAttack", false);
        
    }

    public override void Update()
    {
        base.Update();
        player.SetVelocity(0,0);
        
        if (stateTime <= 0 || finishedAnimation) 
        {
            stateMachine.ChangeState(player.stateIdle);
            return;
        }
        
        Collider2D[] hits = Physics2D.OverlapCircleAll(player.attackCheck.position, player.ScaledAttackRadius());
        foreach (var hit in hits)
        {
            ArrowController arrow = hit.GetComponent<ArrowController>();
            if (arrow)
            {
                arrow.Flip();
                SuccessfulCounterAttack();
            }
            
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy)
            {
                if (enemy.canBeStunned)
                {
                    SuccessfulCounterAttack();
                    CounterAttackEffects(enemy);
                }
            }
        }
    }

    private void SuccessfulCounterAttack()
    {
        stateTime = 10;
        SoundManager.instance.PlaySFX(SfxEffect.PlayerAttack1);
        player.animator.SetBool("SuccessCounterAttack", true);
    }

    private void CounterAttackEffects(Enemy enemy)
    {
        enemy.Stun();
        player.skills.parry.Heal();
        if (canCreateClone)
        {
            player.skills.parry.CreateClone(enemy);
            canCreateClone = false;
        }
    }

    public override void Exit()
    {
        base.Exit();
        
        player.animator.SetBool("SuccessCounterAttack", false);
    }
}
