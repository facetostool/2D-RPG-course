using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Freeze Effect", menuName = "Data/Effect/EnemyFreezeffect")]
public class EnemyFreezeffect : ItemEffect
{
    [SerializeField] private float freezeDuration = 0.1f;
    [SerializeField] private float freezeRadius = 1f;
    public override void Apply(Transform target)
    {
       var playerStats = PlayerManager.instance.player?.stats;
       
       if (playerStats == null)
           return;
        
       if (playerStats.MaxHealthValue() * 0.2f < playerStats.currentHealth)
           return;
        
       Collider2D[] hits = Physics2D.OverlapCircleAll(target.position, freezeRadius);
       foreach (var hit in hits)
       {
           Enemy enemy = hit.GetComponent<Enemy>();
           if (enemy)
           {
               enemy.FreezeFor(freezeDuration);
           }
       }
    }
}
