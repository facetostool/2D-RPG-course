using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStateMove : SkeletonState
{
    public SkeletonStateMove(Enemy _enemy, EnemyStateMachine _stateMachine, string _anim, Skeleton _skeleton) : base(_enemy, _stateMachine, _anim, _skeleton)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        
        if (!skeleton.IsGroundDetected())
        {
            skeleton.SetVelocity(0,0);
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
