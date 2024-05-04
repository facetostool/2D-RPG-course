using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Amulet,
    Armor,
    Weapon,
    Flask
}

[CreateAssetMenu(fileName = "New item data", menuName = "Data/Equipment")]
public class ItemDataEquipment : ItemData
{
    public EquipmentType equipmentType;
    
    [Header("Base Stats")]
    public Stat strength; // 1 point increases damage by 1 and crit power by 1%
    public Stat agility; // 1 point increases evasion by 1 and crit chance by 1%
    public Stat intelligence; // 1 point increases magic damage by 1 and magic resistance by 3
    public Stat vitality; // 1 point increases health by 5

    [Header("Defensive Stats")]
    public Stat armor;
    public Stat evasion;
    public Stat maxHealth;
    public Stat magicResistance;
    
    [Header("Offensive Stats")]
    public Stat damage;
    public Stat critChance;
    public Stat critPower; // 150% by default
    
    [Header("Magic Stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;
    
    public void AddModifiers(EntityStats stats)
    {
        stats.strength.AddModifier(strength.Value());
        stats.agility.AddModifier(agility.Value());
        stats.intelligence.AddModifier(intelligence.Value());
        stats.vitality.AddModifier(vitality.Value());
        
        stats.armor.AddModifier(armor.Value());
        stats.evasion.AddModifier(evasion.Value());
        stats.maxHealth.AddModifier(maxHealth.Value());
        stats.magicResistance.AddModifier(magicResistance.Value());
        
        stats.damage.AddModifier(damage.Value());
        stats.critChance.AddModifier(critChance.Value());
        stats.critPower.AddModifier(critPower.Value());
        
        stats.fireDamage.AddModifier(fireDamage.Value());
        stats.iceDamage.AddModifier(iceDamage.Value());
        stats.lightningDamage.AddModifier(lightningDamage.Value());
        
        stats.onStatsChanged?.Invoke();
    }
    
    public void RemoveModifiers(EntityStats stats)
    {
        stats.strength.RemoveModifier(strength.Value());
        stats.agility.RemoveModifier(agility.Value());
        stats.intelligence.RemoveModifier(intelligence.Value());
        stats.vitality.RemoveModifier(vitality.Value());
        
        stats.armor.RemoveModifier(armor.Value());
        stats.evasion.RemoveModifier(evasion.Value());
        stats.maxHealth.RemoveModifier(maxHealth.Value());
        stats.magicResistance.RemoveModifier(magicResistance.Value());
        
        stats.damage.RemoveModifier(damage.Value());
        stats.critChance.RemoveModifier(critChance.Value());
        stats.critPower.RemoveModifier(critPower.Value());
        
        stats.fireDamage.RemoveModifier(fireDamage.Value());
        stats.iceDamage.RemoveModifier(iceDamage.Value());
        stats.lightningDamage.RemoveModifier(lightningDamage.Value());
        
        stats.onStatsChanged?.Invoke();
    }
}
