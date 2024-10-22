using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherStateJump : ArcherState
{
    public ArcherStateJump(Enemy _enemy, EnemyStateMachine _stateMachine, string _anim, Archer _archer) : base(_enemy, _stateMachine, _anim, _archer)
    {
    }

    public override void Enter()
    {
        base.Enter();

        archer.rb.velocity = new Vector2(archer.rb.velocity.x, archer.jumpForce);
        
        SoundManager.instance.PlaySFX(SfxEffect.ArcherJump);
    }

    public override void Update()
    {
        base.Update();

        if (!archer.IsGroundDetected())
        {
            stateMachine.ChangeState(archer.stateAir);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
