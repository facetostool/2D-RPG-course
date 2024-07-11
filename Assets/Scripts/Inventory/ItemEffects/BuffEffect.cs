using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Buff Effect", menuName = "Data/Effect/BuffEffect")]
public class BuffEffect : ItemEffect
{
    [SerializeField] private float buffDuration;
    [SerializeField] private int buffValue;
    [SerializeField] private StatType stat;
    public override void Apply(Transform target)
    {
        var playerStats = PlayerManager.instance.player?.stats;
        if (playerStats == null)
            return;
        
        playerStats.BuffStatFor(buffDuration, playerStats.GetStat(stat), buffValue);
    }
}
