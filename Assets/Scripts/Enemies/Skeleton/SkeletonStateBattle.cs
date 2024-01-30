using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStateBattle : SkeletonState
{
    public SkeletonStateBattle(Enemy _enemy, EnemyStateMachine _stateMachine, string _anim, Skeleton _skeleton) : base(_enemy, _stateMachine, _anim, _skeleton)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        RaycastHit2D hit = skeleton.PlayerDetectionRaycast();
        if (hit.collider != null && hit.distance <= skeleton.attackDistance)
        {
            stateMachine.ChangeState(skeleton.stateAttack);
            return;
        }
        
        if (IsNeedToFlip())
        {
            skeleton.Flip();
        }
        
        skeleton.SetVelocity(skeleton.moveSpeed*skeleton.moveBattleMultiplayer * skeleton.faceDir, skeleton.rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool IsNeedToFlip()
    {
        var playerPosition = skeleton.player.transform.position;
        var skeletonPosition = skeleton.transform.position;
        return (skeletonPosition.x > playerPosition.x && skeleton.faceDir > 0) ||
               (skeletonPosition.x < playerPosition.x && skeleton.faceDir < 0);
    }
}
