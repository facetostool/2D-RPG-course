using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : EntityState
{
    public Enemy enemy;
    public EnemyStateMachine stateMachine;
    
    public EnemyState(Enemy _enemy, EnemyStateMachine _stateMachine, string _anim):  base(_anim)
    {
        enemy = _enemy;
        stateMachine = _stateMachine;
    }
    
    public override void Enter()
    {
        base.Enter();
        enemy.animator.SetBool(anim, true);
    }
    
    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
        enemy.animator.SetBool(anim, false);
    }
}
