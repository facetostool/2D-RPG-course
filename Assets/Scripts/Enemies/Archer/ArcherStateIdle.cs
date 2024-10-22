using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherStateIdle : ArcherStateGrounded
{
    public ArcherStateIdle(Enemy _enemy, EnemyStateMachine _stateMachine, string _anim, Archer _archer) : base(_enemy, _stateMachine, _anim, _archer)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTime = archer.idleStateTime;
        archer.SetVelocity(0,0);
    }

    public override void Update()
    {
        base.Update();
        
        if (stateTime <= 0)
        {
            archer.Flip();
            stateMachine.ChangeState(archer.stateMove);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
