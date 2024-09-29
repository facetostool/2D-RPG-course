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
    
    protected override void Die()
    {
        base.Die();
        player.Die();
        GetComponent<PlayerItemDrop>().GenerateDrop();
    }

    public override void TakePhysicalDamage(int dmg, bool isCrit)
    {
        base.TakePhysicalDamage(dmg, isCrit);

        if (!InventoryManager.instance.CanUseArmor())
        {
            Debug.Log("Armor is on cooldown!");
            return;
        }
        
        InventoryManager.instance.UseArmor();
    }
    
    public override void TakeDamage(int dmg)
    {
        base.TakeDamage(dmg);
    }

    public override void EvasionEffect(EntityStats attacker, EntityStats target)
    {
        base.EvasionEffect(attacker, target);
        
        player.skills.dodge.CreateClone(attacker.transform);
    }
    
    public void DoCloneDamage(EnemyStats target, float dmgMultiplier)
    {
        if (IsAttackMissed(target))
        {
            target.EvasionEffect(this, target);
            return;
        }
        
        int finalDamage = GetDamage() - target.ArmorValue();
        
        var isCritical = IsCritAttack();
        if (isCritical)
            finalDamage = CalculateCritDamage(finalDamage);
        
        target.TakePhysicalDamage(Mathf.RoundToInt(finalDamage * dmgMultiplier), isCritical);
    }
}
