using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherStateAttack : ArcherState
{
    public ArcherStateAttack(Enemy _enemy, EnemyStateMachine _stateMachine, string _anim, Archer _archer) : base(_enemy, _stateMachine, _anim, _archer)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        archer.SetVelocity(0,0);
        SoundManager.instance.PlaySFX(SfxEffect.ArcherBowLoading, archer.transform);
    }

    public override void Update()
    {
        base.Update();
        
        if (stopAnimations)
        {
            archer.SpawnArrow();
            SoundManager.instance.PlaySFX(SfxEffect.ArcherBowRelease, archer.transform);
            stateMachine.ChangeState(archer.stateBattle);
            return;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
