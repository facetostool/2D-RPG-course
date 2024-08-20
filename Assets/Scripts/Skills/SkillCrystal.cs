using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SkillCrystal : Skill
{
    [SerializeField] GameObject crystalPrefab;
    [SerializeField] float crystalDestroyTime;
    
    [Header("Main")]
    [SerializeField] private SkillTreeSlot crystalSlot;
    [SerializeField] private bool crystalUnlocked;
    
    [Header("Clone")]
    [SerializeField] private SkillTreeSlot cloneSlot;
    [SerializeField] bool cloneUnlocked;
    
    [Header("Explosion")]
    [SerializeField] private SkillTreeSlot explodeSlot;
    [SerializeField] private bool explodeUnlocked;
    
    [Header("Move")]
    [SerializeField] private SkillTreeSlot moveSlot;
    [SerializeField] private bool moveUnlocked;
    [SerializeField] private float moveSpeed;
    
    [Header("Multi crystal")]
    [SerializeField] private SkillTreeSlot multiSlot;
    [SerializeField] private bool isMulti;
    [SerializeField] private int crystalNumber;
    [SerializeField] private List<GameObject> crystalsLeft;
    [SerializeField] private float multiCooldown;
    [SerializeField] private float timeBetweenUseMulti;
    
    GameObject currCrystal;
    
    protected override void Start()
    {
        base.Start();
        
        crystalSlot.onUnlock += () => crystalUnlocked = true;
        cloneSlot.onUnlock += () => cloneUnlocked = true;
        explodeSlot.onUnlock += () => explodeUnlocked = true;
        moveSlot.onUnlock += () => moveUnlocked = true;
        multiSlot.onUnlock += WhenMultiEnabled;
        
        if (isMulti)
        {
            RefillCrystals();
        }
    }

    private void WhenMultiEnabled()
    {
        RefillCrystals();
        isMulti = true;
    }

    protected override void Update()
    {
        base.Update();
    }

    public override bool CanUse()
    {
        return crystalUnlocked && base.CanUse();
    }
    
    public void CreateCrystal(Vector3 position)
    {
        GameObject crystal = Instantiate(crystalPrefab, position, Quaternion.identity);
        crystal.GetComponent<CrystalController>().Setup(crystalDestroyTime, explodeUnlocked, moveUnlocked, moveSpeed, ClosestEnemyPosition(position));
        currCrystal = crystal;
    }

    public override void Use()
    {
        if (isMulti)
        {
            UseMultiCrystal();
            return;
        }
        
        if (!currCrystal)
        {
            var position = player.transform.position;
            GameObject crystal = Instantiate(crystalPrefab, position, Quaternion.identity);
            crystal.GetComponent<CrystalController>().Setup(crystalDestroyTime, explodeUnlocked, moveUnlocked, moveSpeed, ClosestEnemyPosition(position));
            currCrystal = crystal;
            return;
        }

        base.Use();
        if (moveUnlocked)
        {
            return;
        }
        
        // Swap player and crystal positions
        Vector2 playerPosition = player.transform.position;
        player.transform.position = currCrystal.transform.position;
        
        if (cloneUnlocked)
        {
            SkillManager.instance.clone.Use(playerPosition, ClosestEnemyPosition(playerPosition));
            Destroy(currCrystal);
            return;
        }
        
        if (explodeUnlocked)
        {
            currCrystal.transform.position = playerPosition;
            currCrystal.GetComponent<CrystalController>().Explode();
            return;
        }
        
        Destroy(currCrystal);
    }

    public void RefillCrystals()
    {   
        int crystalsLeftCount = crystalsLeft.Count;
        for (int i = 0; i < crystalNumber - crystalsLeftCount; i++)
        {
            crystalsLeft.Add(crystalPrefab);
        }
    }
    
    public void UseMultiCrystal()
    {
       if (crystalsLeft.Count == 0)
            return;
       
       if (crystalsLeft.Count == crystalNumber)
           Invoke(nameof(ResetAbility), timeBetweenUseMulti);

       cooldown = 0;
       int crystalPos = crystalsLeft.Count - 1;
       GameObject crystal = crystalsLeft[crystalPos];
       GameObject crystalGO = Instantiate(crystal, player.transform.position, Quaternion.identity); 
       crystalGO.GetComponent<CrystalController>().Setup(crystalDestroyTime, explodeUnlocked, moveUnlocked, moveSpeed,
           ClosestEnemyPosition(player.transform.position));
       crystalsLeft.Remove(crystal);
       
       if (crystalsLeft.Count == 0)
           ResetAbility();
       
    }

    private void ResetAbility()
    {
        if (cooldownTimer > 0)
            return;
        
        cooldownTimer = multiCooldown;
        RefillCrystals();
        StartImageCooldown();
    }
}
