using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SkillDash : Skill
{
    [SerializeField] public float dashSpeed;
    [SerializeField] public float dashTime;

    [Header("Dash unlock")]
    [SerializeField] public SkillTreeSlot dashSlot;
    [SerializeField] public bool dashUnlocked;
    
    [Header("Clone On Start unlock")]
    [SerializeField] public SkillTreeSlot dashOnStartSlot;
    [SerializeField] public bool cloneOnStartUnlocked;
    
    [Header("Clone On End unlock")]
    [SerializeField] public SkillTreeSlot dashOnEndSlot;
    [SerializeField] public bool cloneOnEndUnlocked;
    
    protected override void Start()
    {
        base.Start();
        
        dashSlot.onUnlock += () => dashUnlocked = true;
        dashOnStartSlot.onUnlock += () => cloneOnStartUnlocked = true;
        dashOnEndSlot.onUnlock += () => cloneOnEndUnlocked = true;
    }

    protected override void Update()
    {
        base.Update();
    }

    public override bool CanUse()
    {
        return dashUnlocked && base.CanUse();
    }

    public void CreateCloneOnStart()
    {
        if (cloneOnStartUnlocked)
            SkillManager.instance.clone.Use(player.transform.position, ClosestEnemyPosition(player.transform.position));
    }
    
    public void CreateCloneOnEnd()
    {
        if (cloneOnEndUnlocked)
            SkillManager.instance.clone.Use(player.transform.position, ClosestEnemyPosition(player.transform.position));
    }
}
