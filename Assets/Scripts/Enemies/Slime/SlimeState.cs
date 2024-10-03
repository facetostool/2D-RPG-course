using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeState : EnemyState
{
    public Slime slime;
    public SlimeState(Enemy _enemy, EnemyStateMachine _stateMachine, string _anim, Slime _slime) : base(_enemy, _stateMachine, _anim)
    {
        slime = _slime;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
