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
        GetComponent<PlayerItemDrop>().GenerateDrop();
    }

    public override void TakePhysicalDamage(int dmg)
    {
        base.TakePhysicalDamage(dmg);

        if (!InventoryManager.instance.CanUseArmor())
        {
            Debug.Log("Armor is on cooldown!");
            return;
        }
        
        InventoryManager.instance.UseArmor();
    }
}
