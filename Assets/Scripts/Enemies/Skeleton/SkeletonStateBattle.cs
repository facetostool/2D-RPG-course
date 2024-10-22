using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStateBattle : SkeletonState
{

    private float attackCooldownTimer;
    
    public SkeletonStateBattle(Enemy _enemy, EnemyStateMachine _stateMachine, string _anim, Skeleton _skeleton) : base(_enemy, _stateMachine, _anim, _skeleton)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        SoundManager.instance.PlaySFX( SfxEffect.MonsterGrowl1);
        stateTime = skeleton.battleNoDetectionTime;
        
        if (PlayerManager.instance.player.IsDead())
        {
                stateMachine.ChangeState(skeleton.stateIdle);
        }
    }

    public override void Update()
    {
        base.Update();
        
        if (attackCooldownTimer > 0)
            attackCooldownTimer -= Time.deltaTime;
        
        if (enemy.isKnocked)
            return;
        
        if (IsNeedToFlip())
            skeleton.Flip();
        
        if (stateTime <= 0)
        {
            stateMachine.ChangeState(skeleton.stateIdle);
        }
        
        RaycastHit2D hit = skeleton.PlayerAttackRaycast();
        if (hit.collider)
        {
            stateTime = skeleton.battleNoDetectionTime;
            if (!(attackCooldownTimer <= 0)) return;
            attackCooldownTimer = Random.Range(enemy.attackMinCooldown, enemy.attackMaxCooldown);
            stateMachine.ChangeState(skeleton.stateAttack);
            return;
        }

        skeleton.SetVelocity(skeleton.moveSpeed*skeleton.moveBattleMultiplayer * skeleton.faceDir, skeleton.rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool IsNeedToFlip()
    {
        var playerPosition = PlayerManager.instance.player.transform.position;
        var skeletonPosition = skeleton.transform.position;
        return (skeletonPosition.x > playerPosition.x && skeleton.faceDir > 0) ||
               (skeletonPosition.x < playerPosition.x && skeleton.faceDir < 0);
    }
}
