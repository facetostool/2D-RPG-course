using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : EntityStats
{
    Player player;
    public override void Start()
    {
        base.Start();
        
        player = GetComponent<Player>();
    }

    
    public override void DoDamage(EntityStats target)
    {
        base.DoDamage(target);
    }
    
    protected override void Die()
    {
        base.Die();
        player.Die();
    }
}
