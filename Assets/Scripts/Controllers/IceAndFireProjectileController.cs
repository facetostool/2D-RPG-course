using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceAndFireProjectileController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy"))
            return;
    
        PlayerManager.instance.player.stats.DoMagicDamage(other.GetComponent<Enemy>().stats);
    }
}
