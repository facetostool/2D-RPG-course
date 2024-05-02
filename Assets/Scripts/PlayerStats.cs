using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    Player player;
    public override void Start()
    {
        base.Start();
        
        player = GetComponent<Player>();
    }

    public override void DoDamage(CharacterStats target)
    {
        base.DoDamage(target);
    }
    
    protected override void Die()
    {
        base.Die();
        player.Die();
    }
}
