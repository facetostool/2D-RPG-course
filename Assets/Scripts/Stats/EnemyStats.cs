using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : EntityStats
{
    private Enemy enemy;
    private ItemDrop itemDropSystem;
    
    [Header("Level settings")]
    [SerializeField] private int level = 1;
    [Range(0, 1f)]
    [SerializeField] private float levelStatsModifier = 0.4f;
    
    [SerializeField] private Stat soulsToDrop;
    
    public override void Start()
    {   
        soulsToDrop.SetBaseValue(100);
        AddLevelModifiers();
        base.Start();
        
        enemy = GetComponent<Enemy>();
        itemDropSystem = GetComponent<ItemDrop>();
    }

    private void Modify(Stat _stat)
    {
        for (int i = 1; i < level; i++)
        {
            _stat.AddModifier(Mathf.RoundToInt(_stat.Value() * levelStatsModifier));
        }
    }
    
    private void  AddLevelModifiers()
    {
        Modify(strength);
        Modify(agility);
        Modify(intelligence);
        Modify(vitality);
        
        Modify(armor);
        Modify(evasion);
        Modify(maxHealth);
        Modify(magicResistance);
        
        Modify(damage);
        Modify(critChance);
        Modify(critPower);
        
        Modify(fireDamage);
        Modify(iceDamage);
        Modify(lightningDamage);

        Modify(soulsToDrop);
    }

    public override void TakePhysicalDamage(int dmg)
    {
        base.TakePhysicalDamage(dmg);
    }
    
    protected override void Die()
    {
        base.Die();
        enemy.Die();
        
        itemDropSystem.GenerateDrop();
        PlayerManager.instance.AddCurrency(soulsToDrop.Value());
    }
}
