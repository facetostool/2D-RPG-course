using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateAttack : PlayerState
{
    private int attackCounter = 0;
    private float lastAttackTime = 0;
    
    public PlayerStateAttack(Player _player, PlayerStateMachine _stateMachine, string _anim) : base(_player, _stateMachine, _anim)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTime = 0.1f;

        float attackDir = player.faceDir;
        if (player.moveVector.x != 0)
        {
            attackDir = player.moveVector.x;
        }

        if (Time.time - lastAttackTime > player.comboTimer)
        {
            attackCounter = 0;
        }
        
        player.SetVelocity(player.attackEnterForce[attackCounter].x * attackDir, player.attackEnterForce[attackCounter].y);
        
        attackCounter++;
        player.animator.SetInteger("AttackCounter", attackCounter);
        if (attackCounter >= 3)
        {
            attackCounter = 0;
        }
    }

    public override void Update()
    {
        base.Update();

        if (stateTime <= 0)
        {
            player.SetVelocity(0,0);
        }
        
        if (stopAnimations)
        {
            stateMachine.ChangeState(player.stateIdle);
            return;
        }
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", 0.1f);
        lastAttackTime = Time.time;
    }
}
