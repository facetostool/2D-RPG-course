using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public EnemyStateMachine stateMachine;
    
    public override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        
        stateMachine.currentState.Update();
    }

    public void OnHitAttackTrigger()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(this.attackCheck.position, this.attackCheckRadius);
        foreach (var hit in hits)
        {
            Player player = hit.GetComponent<Player>();
            if (player)
            {
                player.Damage();
            }
        }
    } 
}
