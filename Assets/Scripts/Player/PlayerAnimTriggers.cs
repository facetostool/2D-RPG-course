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

    public void FinishAnimationTrigger()
    {
        _player.stateMachine.currentState.FinishedAnimationsTrigger();
    }

    public void OnHitAttackTrigger()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(_player.attackCheck.position, _player.ScaledAttackRadius());
        foreach (var hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (!enemy) continue;
            
            _player.stats.DoDamage(enemy.stats, _player.transform);

            if (!InventoryManager.instance.equipmentItemDict.TryGetValue(EquipmentType.Weapon, out var weapon))
                continue;
            if (weapon == null) continue;
            var itemData = weapon.itemData as ItemDataEquipment;
            itemData?.ApplyEffects(enemy.transform);
        }
    }
}
