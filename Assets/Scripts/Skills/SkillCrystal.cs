using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCrystal : Skill
{
    [SerializeField] GameObject crystalPrefab;
    [SerializeField] float crystalDestroyTime;
    
    [Header("Explosion")]
    [SerializeField] private bool isExplode;
    
    [Header("Move")]
    [SerializeField] private bool canMove;
    [SerializeField] private float moveSpeed;
    
    GameObject currCrystal;

    public override void Use()
    {
        base.Use();
        
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

        Vector2 playerPosition = player.transform.position;
        player.transform.position = currCrystal.transform.position;
        if (isExplode)
        {
            currCrystal.transform.position = playerPosition;
            currCrystal.GetComponent<CrystalController>().Explode();
            return;
        }
        
        Destroy(currCrystal);
    }
}
