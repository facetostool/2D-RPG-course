using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New thunder hit", menuName = "Data/Effect/IceAndFireHit")]
public class IceAndFireHitEffect : ItemEffect
{
    [SerializeField] private GameObject effectPrefab;
    [SerializeField] private float projectileSpeed = 5f;
    public override void Apply(Transform target)
    {
        var player = PlayerManager.instance.player;
        if (player == null)
            return;
        
        if (player.GetAttackCounter() != 0) 
            return;
        
        GameObject projectile = Instantiate(effectPrefab, target.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileSpeed * player.faceDir, 0f);
        Destroy(projectile, 1f);
    }
}
