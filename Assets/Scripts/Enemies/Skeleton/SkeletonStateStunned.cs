using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStateStunned : SkeletonState
{
    public SkeletonStateStunned(Enemy _enemy, EnemyStateMachine _stateMachine, string _anim, Skeleton _skeleton) : base(_enemy, _stateMachine, _anim, _skeleton)
    {
    }

    public override void Enter()
    {
        base.Enter();

        skeleton.fx.InvokeRepeating(nameof(EntityFlashFX.StunnedEffect), 0, 0.1f);
        stateTime = skeleton.stunnedDuration;
        skeleton.SetVelocity(skeleton.stunnedDirection.x * -skeleton.faceDir, skeleton.stunnedDirection.y);
    }

    public override void Update()
    {
        base.Update();
        
        if (stateTime <= 0)
        {
            skeleton.SetVelocity(0,0);
            stateMachine.ChangeState(skeleton.stateIdle);
        }
    }

    public override void Exit()
    {
        base.Exit();
        
        skeleton.fx.StopStunnedEffect();
    }
}
