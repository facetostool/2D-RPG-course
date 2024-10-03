using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeStateIdle : SlimeStateGrounded
{
    public SlimeStateIdle(Enemy _enemy, EnemyStateMachine _stateMachine, string _anim, Slime _slime) : base(_enemy, _stateMachine, _anim, _slime)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTime = slime.idleStateTime;
        slime.SetVelocity(0,0);
    }

    public override void Update()
    {
        base.Update();
        
        if (stateTime <= 0)
        {
            slime.Flip();
            stateMachine.ChangeState(slime.stateMove);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
