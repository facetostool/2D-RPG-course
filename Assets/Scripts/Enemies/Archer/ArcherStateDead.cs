using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherStateDead : ArcherState
{
    public ArcherStateDead(Enemy _enemy, EnemyStateMachine _stateMachine, string _anim, Archer _archer) : base(_enemy, _stateMachine, _anim, _archer)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
         SoundManager.instance.PlaySFX(SfxEffect.ArcherDie, archer.transform);
    }

    public override void Update()
    {
        base.Update();
        
        archer.SetVelocity(0,0);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
