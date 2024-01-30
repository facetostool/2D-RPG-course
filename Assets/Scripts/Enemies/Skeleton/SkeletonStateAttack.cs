using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStateAttack : SkeletonState
{
    public SkeletonStateAttack(Enemy _enemy, EnemyStateMachine _stateMachine, string _anim, Skeleton _skeleton) : base(_enemy, _stateMachine, _anim, _skeleton)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        skeleton.SetVelocity(0,0);
    }

    public override void Update()
    {
        base.Update();
        
        if (stopAnimations)
        {
            stateMachine.ChangeState(skeleton.stateBattle);
            return;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
