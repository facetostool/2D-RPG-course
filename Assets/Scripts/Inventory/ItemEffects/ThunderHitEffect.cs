using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New thunder hit", menuName = "Data/Effect/ThunderHit")]
public class ThunderHitEffect : ItemEffect
{
    [SerializeField] private GameObject thunderPrefab;
    public override void Apply(Transform target)
    {
        GameObject thunder = Instantiate(thunderPrefab, target.position, Quaternion.identity);
        Destroy(thunder, 1f);
    }
}
