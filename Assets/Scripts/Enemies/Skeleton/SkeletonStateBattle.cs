using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

public class SkeletonStateBattle : SkeletonState
{
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
            return;
        }
    }

    public override void Update()
    {
        base.Update();
        
        if (enemy.isKnocked)
        {
            return;
        }
        
        RaycastHit2D hit = skeleton.PlayerDetectionRaycast();
        if (hit.collider != null)
        {
            stateTime = skeleton.battleNoDetectionTime;
            if (hit.distance <= skeleton.attackDistance)
            {
                stateMachine.ChangeState(skeleton.stateAttack);
                return;
            }
        }
        
        if (IsNeedToFlip())
        {
            skeleton.Flip();
        }
        
        skeleton.SetVelocity(skeleton.moveSpeed*skeleton.moveBattleMultiplayer * skeleton.faceDir, skeleton.rb.velocity.y);

        if (stateTime <= 0)
        {
            stateMachine.ChangeState(skeleton.stateIdle);
        }
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
