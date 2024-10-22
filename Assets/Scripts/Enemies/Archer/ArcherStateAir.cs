using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherStateAir : ArcherState
{
    public ArcherStateAir(Enemy _enemy, EnemyStateMachine _stateMachine, string _anim, Archer _archer) : base(_enemy, _stateMachine, _anim, _archer)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        archer.animator.SetFloat("yVelocity", archer.rb.velocity.y);
        
        if (archer.IsGroundDetected())
        {
            SoundManager.instance.PlaySFX(SfxEffect.Landing);
            stateMachine.ChangeState(archer.stateIdle);
            return;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
