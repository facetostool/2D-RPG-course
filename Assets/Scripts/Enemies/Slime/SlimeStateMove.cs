using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeStateMove : SlimeStateGrounded
{
    public SlimeStateMove(Enemy _enemy, EnemyStateMachine _stateMachine, string _anim, Slime _slime) : base(_enemy, _stateMachine, _anim, _slime)
    {
    }

    public override void Enter()
    {
        base.Enter();
        // SoundManager.instance.PlaySFX(24, slime.transform);
    }

    public override void Update()
    {
        base.Update();

        
        if (!slime.IsGroundDetected())
        {
            stateMachine.ChangeState(slime.stateIdle);
            return;
        }
        
        slime.SetVelocity(slime.moveSpeed * slime.faceDir, slime.rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
