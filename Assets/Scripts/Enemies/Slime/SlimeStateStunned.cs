using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeStateStunned : SlimeState
{
    float knockbackTime;
    public SlimeStateStunned(Enemy _enemy, EnemyStateMachine _stateMachine, string _anim, Slime _slime) : base(_enemy, _stateMachine, _anim, _slime)
    {
        
    }

    public override void Enter()
    {
        base.Enter();

        slime.fx.InvokeRepeating(nameof(EntityFX.StunnedEffect), 0, 0.1f);
        stateTime = slime.stunnedDuration;
        knockbackTime = slime.stunnedKnockDuration;
    }

    public override void Update()
    {
        base.Update();
        
        if (knockbackTime > 0)
        {
            slime.SetVelocity(slime.stunnedDirection.x * -slime.faceDir, slime.stunnedDirection.y);
            knockbackTime -= Time.deltaTime;
        } else
        {
            slime.SetVelocity(0, slime.rb.velocity.y);
        }

        if (stateTime <= 0)
        {
            stateMachine.ChangeState(slime.stateBattle);
        }
    }

    public override void Exit()
    {
        base.Exit();
        
        slime.fx.StopEffect();
    }
}
