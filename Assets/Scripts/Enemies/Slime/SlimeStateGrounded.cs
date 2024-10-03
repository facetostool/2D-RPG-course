using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeStateGrounded : SlimeState
{
    public SlimeStateGrounded(Enemy _enemy, EnemyStateMachine _stateMachine, string _anim, Slime _slime) : base(_enemy, _stateMachine, _anim, _slime)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        
        if (slime.PlayerDetectionRaycast() || Vector3.Distance(slime.transform.position, PlayerManager.instance.player.transform.position) < 2 && !PlayerManager.instance.player.IsDead())
        {
            stateMachine.ChangeState(slime.stateBattle);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
