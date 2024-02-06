using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDash : Skill
{
    [SerializeField] public float dashSpeed;
    [SerializeField] public float dashTime;

    [SerializeField] public GameObject clone;
    [SerializeField] public float disappearSpeed;
    
    public enum DashType
    {
        Regular,
        Clone
    }
    
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

    public override void Use()
    {
        base.Use();

        GameObject cloneObject = Instantiate(clone, player.transform.position, Quaternion.identity);
        cloneObject.GetComponent<CloneController>().Setup(disappearSpeed);
    }
}
