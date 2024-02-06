using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStateGrounded : SkeletonState
{
    public SkeletonStateGrounded(Enemy _enemy, EnemyStateMachine _stateMachine, string _anim, Skeleton _skeleton) : base(_enemy, _stateMachine, _anim, _skeleton)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        
        if (skeleton.PlayerDetectionRaycast() || Vector3.Distance(skeleton.transform.position, PlayerManager.instance.player.transform.position) < 2)
        {
            stateMachine.ChangeState(skeleton.stateBattle);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
