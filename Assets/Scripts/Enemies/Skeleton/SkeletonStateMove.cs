using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStateMove : SkeletonStateGrounded
{
    public SkeletonStateMove(Enemy _enemy, EnemyStateMachine _stateMachine, string _anim, Skeleton _skeleton) : base(_enemy, _stateMachine, _anim, _skeleton)
    {
    }

    public override void Enter()
    {
        base.Enter();
        // SoundManager.instance.PlaySFX(24, skeleton.transform);
    }

    public override void Update()
    {
        base.Update();

        
        if (!skeleton.IsGroundDetected())
        {
            stateMachine.ChangeState(skeleton.stateIdle);
            return;
        }
        
        skeleton.SetVelocity(skeleton.moveSpeed * skeleton.faceDir, skeleton.rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
