using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillThrowSword : Skill
{
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 throwForce;
    [SerializeField] private float gravityScale;
    
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
        
        GameObject swordObject = Instantiate(swordPrefab, player.transform.position, Quaternion.identity);
        swordObject.GetComponent<SwordController>().Setup(throwForce, gravityScale);
    }
}
