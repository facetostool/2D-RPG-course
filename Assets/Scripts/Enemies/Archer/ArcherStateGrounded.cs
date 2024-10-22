using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherStateGrounded : ArcherState
{
    public ArcherStateGrounded(Enemy _enemy, EnemyStateMachine _stateMachine, string _anim, Archer _archer) : base(_enemy, _stateMachine, _anim, _archer)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        
        if (archer.PlayerDetectionRaycast() || Vector3.Distance(archer.transform.position, PlayerManager.instance.player.transform.position) < 2 && !PlayerManager.instance.player.IsDead())
        {
            stateMachine.ChangeState(archer.stateBattle);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
