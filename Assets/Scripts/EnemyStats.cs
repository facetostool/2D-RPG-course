using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    private Enemy enemy;
    public override void Start()
    {
        base.Start();
        
        enemy = GetComponent<Enemy>();
    }

    public override void TakeDamage(int dmg)
    {
        base.TakeDamage(dmg);
    }
    
    protected override void Die()
    {
        base.Die();
        enemy.Die();
    }
}
