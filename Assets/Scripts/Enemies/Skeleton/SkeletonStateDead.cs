using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStateDead : SkeletonState
{
    public SkeletonStateDead(Enemy _enemy, EnemyStateMachine _stateMachine, string _anim, Skeleton _skeleton) : base(_enemy, _stateMachine, _anim, _skeleton)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        
        skeleton.SetVelocity(0,0);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
