using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherStateMove : ArcherStateGrounded
{
    public ArcherStateMove(Enemy _enemy, EnemyStateMachine _stateMachine, string _anim, Archer _archer) : base(_enemy, _stateMachine, _anim, _archer)
    {
    }

    public override void Enter()
    {
        base.Enter();
        // SoundManager.instance.PlaySFX(24, archer.transform);
    }

    public override void Update()
    {
        base.Update();

        
        if (!archer.IsGroundDetected())
        {
            stateMachine.ChangeState(archer.stateIdle);
            return;
        }
        
        archer.SetVelocity(archer.moveSpeed * archer.faceDir, archer.rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
