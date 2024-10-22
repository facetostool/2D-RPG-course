using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherState : EnemyState
{
    public Archer archer;
    public ArcherState(Enemy _enemy, EnemyStateMachine _stateMachine, string _anim, Archer _archer) : base(_enemy, _stateMachine, _anim)
    {
        archer = _archer;
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
