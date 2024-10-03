using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeStateBattle : SlimeState
{

    private float attackCooldownTimer;
    
    public SlimeStateBattle(Enemy _enemy, EnemyStateMachine _stateMachine, string _anim, Slime _slime) : base(_enemy, _stateMachine, _anim, _slime)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        // SoundManager.instance.PlaySFX( SfxEffect.MonsterGrowl1);
        stateTime = slime.battleNoDetectionTime;
        
        if (PlayerManager.instance.player.IsDead())
        {
                stateMachine.ChangeState(slime.stateIdle);
        }
    }

    public override void Update()
    {
        base.Update();
        
        if (attackCooldownTimer > 0)
            attackCooldownTimer -= Time.deltaTime;
        
        if (enemy.isKnocked)
            return;
        
        RaycastHit2D hit = slime.PlayerDetectionRaycast();
        if (hit.collider != null)
        {
            stateTime = slime.battleNoDetectionTime;
            if (OnAttackDistance())
            {
                if (attackCooldownTimer <= 0)
                {
                    attackCooldownTimer = Random.Range(enemy.attackMinCooldown, enemy.attackMaxCooldown);
                    stateMachine.ChangeState(slime.stateAttack);
                    return;
                }
            }
        }
        
        if (IsNeedToFlip())
            slime.Flip();
        
        slime.SetVelocity(slime.moveSpeed*slime.moveBattleMultiplayer * slime.faceDir, slime.rb.velocity.y);

        if (stateTime <= 0)
        {
            stateMachine.ChangeState(slime.stateIdle);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool IsNeedToFlip()
    {
        var playerPosition = PlayerManager.instance.player.transform.position;
        var slimePosition = slime.transform.position;
        return (slimePosition.x > playerPosition.x && slime.faceDir > 0) ||
               (slimePosition.x < playerPosition.x && slime.faceDir < 0);
    }
    
    private bool OnAttackDistance()
    {
        return Vector2.Distance(enemy.transform.position, PlayerManager.instance.player.transform.position) <= slime.attackDistance;
    }
}
