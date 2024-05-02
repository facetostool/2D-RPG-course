using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimTriggers : MonoBehaviour
{
    private Player _player;
    
    void Start()
    {
        _player = GetComponentInParent<Player>();
    }
    
    void Update()
    {
        
    }

    public void CancelAnimationTrigger()
    {
        _player.stateMachine.currentState.StopAnimationsTrigger();
    }

    public void OnHitAttackTrigger()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(_player.attackCheck.position, _player.attackCheckRadius);
        foreach (var hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy)
            {
                _player.stats.DoDamage(enemy.stats);
            }
        }
    }
}
