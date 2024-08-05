using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDodge : Skill
{
    [Header("Dodge unlock")]
    [SerializeField] public SkillTreeSlot dodgeSlot;
    [SerializeField] public bool dodgeUnlocked;
    [SerializeField] public int bonusEvasion;
    
    [Header("Dodge Clone")]
    [SerializeField] public SkillTreeSlot dodgeCloneSlot;
    [SerializeField] public bool dodgeCloneUnlocked;
    
    protected override void Start()
    {
        base.Start();
        
        dodgeSlot.onUnlock += OnDodgeUnlocked;
        dodgeCloneSlot.onUnlock += () => dodgeCloneUnlocked = true;
    }

    private void OnDodgeUnlocked()
    {
        dodgeUnlocked = true;
        player.stats.AddModifier(StatType.evasion, bonusEvasion);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override bool CanUse()
    {
        return dodgeUnlocked && base.CanUse();
    }

    public override void Use()
    {
        base.Use();
    }
    
    public void CreateClone(Transform target)
    {
        Debug.Log("CreateClone");
        if (dodgeCloneUnlocked)
        {
            Debug.Log("CreateClone2");
            player.skills.clone.UseWithDelayAndOffset(target);
        }
            
    }
}
