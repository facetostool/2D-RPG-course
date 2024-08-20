using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemDrop : ItemDrop
{
    [Header("Player Item Drop")]
    [Range(0, 100)]
    [SerializeField] private int chanceToDropEquipment;
    
    public override void GenerateDrop()
    {
        List<InventoryItem> randomItemsToDrop = new List<InventoryItem>();
        foreach (var item in InventoryManager.instance.equipmentItems)
        {
            if (Random.Range(0, 100) < chanceToDropEquipment)
            {
                randomItemsToDrop.Add(item);
            }
        }
        
        for (int i = 0; i < randomItemsToDrop.Count; i++)
        {
            DropItem(randomItemsToDrop[i].itemData);
            InventoryManager.instance.UnequipItem(randomItemsToDrop[i].itemData);
            InventoryManager.instance.RemoveItem(randomItemsToDrop[i].itemData);
        }
    }
}
