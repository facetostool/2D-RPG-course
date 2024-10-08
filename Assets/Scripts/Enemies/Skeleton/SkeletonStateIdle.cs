using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStateIdle : SkeletonStateGrounded
{
    public SkeletonStateIdle(Enemy _enemy, EnemyStateMachine _stateMachine, string _anim, Skeleton _skeleton) : base(_enemy, _stateMachine, _anim, _skeleton)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTime = skeleton.idleStateTime;
        skeleton.SetVelocity(0,0);
    }

    public override void Update()
    {
        base.Update();
        
        if (stateTime <= 0)
        {
            skeleton.Flip();
            stateMachine.ChangeState(skeleton.stateMove);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
