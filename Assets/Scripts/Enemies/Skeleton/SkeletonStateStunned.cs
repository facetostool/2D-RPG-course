using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStateStunned : SkeletonState
{
    float knockbackTime;
    public SkeletonStateStunned(Enemy _enemy, EnemyStateMachine _stateMachine, string _anim, Skeleton _skeleton) : base(_enemy, _stateMachine, _anim, _skeleton)
    {
        
    }

    public override void Enter()
    {
        base.Enter();

        skeleton.fx.InvokeRepeating(nameof(EntityFX.StunnedEffect), 0, 0.1f);
        stateTime = skeleton.stunnedDuration;
        knockbackTime = skeleton.stunnedKnockDuration;
    }

    public override void Update()
    {
        base.Update();
        
        if (knockbackTime > 0)
        {
            skeleton.SetVelocity(skeleton.stunnedDirection.x * -skeleton.faceDir, skeleton.stunnedDirection.y);
            knockbackTime -= Time.deltaTime;
        } else
        {
            skeleton.SetVelocity(0, skeleton.rb.velocity.y);
        }

        if (stateTime <= 0)
        {
            stateMachine.ChangeState(skeleton.stateBattle);
        }
    }

    public override void Exit()
    {
        base.Exit();
        
        skeleton.fx.StopEffect();
    }
}
