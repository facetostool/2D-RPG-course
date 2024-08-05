using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillParry : Skill
{
    [Header("Parry unlock")]
    [SerializeField] public SkillTreeSlot parrySlot;
    [SerializeField] public bool parryUnlocked;
    
    [Header("Parry Heal unlock")]
    [SerializeField] public SkillTreeSlot parryHealSlot;
    [SerializeField] public bool parryHealUnlocked;
    [Range(0, 1)]
    [SerializeField] public float healAmount;
    
    [Header("Parry Clone unlock")]
    [SerializeField] public SkillTreeSlot parryCloneSlot;
    [SerializeField] public bool parryCloneUnlocked;
    
    protected override void Start()
    {
        base.Start();
        
        parrySlot.onUnlock += () => parryUnlocked = true;
        parryHealSlot.onUnlock += () => parryHealUnlocked = true;
        parryCloneSlot.onUnlock += () => parryCloneUnlocked = true;
    }

    protected override void Update()
    {
        base.Update();
    }

    public override bool CanUse()
    {
        return parryUnlocked && base.CanUse();
    }

    public override void Use()
    {
        base.Use();
    }
    
    public void Heal()
    {
        if (parryHealUnlocked)
            player.stats.HealByMax(healAmount);
    }
    
    public void CreateClone(Enemy hit)
    {
        if (parryCloneUnlocked)
            player.skills.clone.UseWithDelayAndOffset(hit.transform);
    }
}
