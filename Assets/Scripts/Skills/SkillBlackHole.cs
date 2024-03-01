using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBlackHole : Skill
{
    [SerializeField] private GameObject blackHolePrefab;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;
    [SerializeField] private float maxSize;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float attackNumber;
    
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

        GameObject blackHole = Instantiate(blackHolePrefab, player.transform.position, Quaternion.identity);
        blackHole.GetComponent<BlackHoleController>().Setup(growSpeed, shrinkSpeed, maxSize, attackCooldown, attackNumber);
    }
}
