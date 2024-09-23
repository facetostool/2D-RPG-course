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
    [SerializeField] private float skillTimer;
    [SerializeField] public float flySpeed;
    [SerializeField] public float flyTime;
    [SerializeField] private float backRotateSpeed;
    
    [SerializeField] private SkillTreeSlot blackHoleSlot;
    [SerializeField] private bool blackHoleUnlocked;
    
    protected override void Start()
    {
        base.Start();
        
        blackHoleSlot.onUnlock += () => blackHoleUnlocked = true;
    }

    protected override void Update()
    {
        base.Update();
    }

    public override bool CanUse()
    {
        return blackHoleUnlocked && base.CanUse();
    }

    public override void Use()
    {
        base.Use();

        GameObject blackHole = Instantiate(blackHolePrefab, player.transform.position, Quaternion.identity);
        blackHole.GetComponent<BlackHoleController>().Setup(growSpeed, shrinkSpeed, maxSize, attackCooldown, attackNumber, skillTimer, backRotateSpeed);
    }
}
