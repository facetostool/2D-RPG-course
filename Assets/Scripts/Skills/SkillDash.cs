using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDash : Skill
{
    [SerializeField] public float dashSpeed;
    [SerializeField] public float dashTime;

    [SerializeField] public bool cloneOnStart;
    [SerializeField] public bool cloneOnEnd;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override bool CanUse()
    {
        return base.CanUse();
    }

    public void Use(string eventType = "")
    {
        base.Use();

        switch (eventType)
        {
            case "start":
                CreateCloneOnStart();
                break;
            case "end":
                CreateCloneOnEnd();
                break;
        }
    }

    public void CreateCloneOnStart()
    {
        if (cloneOnStart)
            SkillManager.instance.clone.Use(player.transform.position, ClosestEnemyPosition(player.transform.position));
    }
    
    public void CreateCloneOnEnd()
    {
        if (cloneOnEnd)
            SkillManager.instance.clone.Use(player.transform.position, ClosestEnemyPosition(player.transform.position));
    }
}
