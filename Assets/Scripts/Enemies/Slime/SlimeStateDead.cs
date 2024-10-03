using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeStateDead : SlimeState
{
    public SlimeStateDead(Enemy _enemy, EnemyStateMachine _stateMachine, string _anim, Slime _slime) : base(_enemy, _stateMachine, _anim, _slime)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        // SoundManager.instance.PlaySFX(SfxEffect.SlimeDie);
    }

    public override void Update()
    {
        base.Update();
        
        slime.SetVelocity(0,0);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
