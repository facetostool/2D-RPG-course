using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal Effect", menuName = "Data/Effect/HealEffect")]
public class HealEffect : ItemEffect
{
    [Range(0, 1)]
    [SerializeField] private float healPercentage = 0.1f;
    public override void Apply(Transform target)
    {
        var playerStats = PlayerManager.instance.player?.stats;
        if (playerStats == null)
            return;
        
        playerStats.HealByMax(healPercentage);
    }
}
