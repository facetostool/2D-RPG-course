using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeStateAttack : SlimeState
{
    public SlimeStateAttack(Enemy _enemy, EnemyStateMachine _stateMachine, string _anim, Slime _slime) : base(_enemy, _stateMachine, _anim, _slime)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        slime.SetVelocity(0,0);
        // SoundManager.instance.PlaySFX(SfxEffect.SlimeAttack1, slime.transform,  0.3f);
    }

    public override void Update()
    {
        base.Update();
        
        if (stopAnimations)
        {
            stateMachine.ChangeState(slime.stateBattle);
            return;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
