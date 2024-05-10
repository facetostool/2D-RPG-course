using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SkillCrystal : Skill
{
    [SerializeField] GameObject crystalPrefab;
    [SerializeField] float crystalDestroyTime;
    [SerializeField] bool canCreateClone;
    
    [Header("Explosion")]
    [SerializeField] private bool isExplode;
    
    [Header("Move")]
    [SerializeField] private bool canMove;
    [SerializeField] private float moveSpeed;
    
    [Header("Multi crystal")]
    [SerializeField] private bool isMulti;
    [SerializeField] private int crystalNumber;
    [SerializeField] private List<GameObject> crystalsLeft;
    [SerializeField] private float multiCooldown;
    [SerializeField] private float timeBetweenUseMulti;
    
    GameObject currCrystal;
    
    protected override void Start()
    {
        base.Start();
        
        if (isMulti)
        {
            RefillCrystals();
        }
    }

    protected override void Update()
    {
        base.Update();
    }


    public override void Use()
    {
        base.Use();
        
        if (isMulti)
        {
            UseMultiCrystal();
            return;
        }
        
        if (!currCrystal)
        {
            var position = player.transform.position;
            GameObject crystal = Instantiate(crystalPrefab, position, Quaternion.identity);
            crystal.GetComponent<CrystalController>().Setup(crystalDestroyTime, isExplode, canMove, moveSpeed, ClosestEnemyPosition(position));
            currCrystal = crystal;
            return;
        }

        if (canMove)
        {
            return;
        }

        // Swap player and crystal positions
        Vector2 playerPosition = player.transform.position;
        player.transform.position = currCrystal.transform.position;
        
        if (canCreateClone)
        {
            SkillManager.instance.clone.Use(playerPosition, ClosestEnemyPosition(playerPosition));
            Destroy(currCrystal);
            return;
        }
        
        if (isExplode)
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
       crystalGO.GetComponent<CrystalController>().Setup(crystalDestroyTime, isExplode, canMove, moveSpeed,
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
    }
}
