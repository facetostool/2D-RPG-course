using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherStateBattle : ArcherState
{

    private float attackCooldownTimer;
    
    public ArcherStateBattle(Enemy _enemy, EnemyStateMachine _stateMachine, string _anim, Archer _archer) : base(_enemy, _stateMachine, _anim, _archer)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        // SoundManager.instance.PlaySFX( SfxEffect.MonsterGrowl1);
        stateTime = archer.battleNoDetectionTime;
        
        if (PlayerManager.instance.player.IsDead())
        {
                stateMachine.ChangeState(archer.stateIdle);
        }
    }

    public override void Update()
    {
        base.Update();
        
        if (attackCooldownTimer > 0)
            attackCooldownTimer -= Time.deltaTime;
        
        if (enemy.isKnocked)
            return;
        
        if (IsNeedToFlip())
            archer.Flip();

        if (stateTime <= 0)
        {
            stateMachine.ChangeState(archer.stateIdle);
        }
        
        RaycastHit2D hit = archer.PlayerAttackRaycast();
        if (hit.collider)
        {
            stateTime = archer.battleNoDetectionTime;
            if (!(attackCooldownTimer <= 0)) return;
            attackCooldownTimer = Random.Range(enemy.attackMinCooldown, enemy.attackMaxCooldown);
            stateMachine.ChangeState(archer.stateAttack);
            return;
        }

        archer.SetVelocity(archer.moveSpeed*archer.moveBattleMultiplayer * archer.faceDir, archer.rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool IsNeedToFlip()
    {
        var playerPosition = PlayerManager.instance.player.transform.position;
        var archerPosition = archer.transform.position;
        return (archerPosition.x > playerPosition.x && archer.faceDir > 0) ||
               (archerPosition.x < playerPosition.x && archer.faceDir < 0);
    }
    
    private bool OnAttackDistance()
    {
        return Vector2.Distance(archer.playerCheck.transform.position, PlayerManager.instance.player.transform.position) <= archer.ScaledAttackDistance();
    }
}
