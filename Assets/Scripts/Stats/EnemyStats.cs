using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : EntityStats
{
    private Enemy enemy;
    public override void Start()
    {   
        base.Start();
        
        enemy = GetComponent<Enemy>();
    }

    public override void TakePhysicalDamage(int dmg)
    {
        base.TakePhysicalDamage(dmg);
    }
    
    protected override void Die()
    {
        base.Die();
        enemy.Die();
    }
}
