using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryItem {
    public ItemData itemData;
    public int amount;

    public InventoryItem(ItemData item)
    {
        this.itemData = item;
        AddAmount(1);
    }
    
    public void AddAmount(int amount)
    {
        this.amount += amount;
    }
    
    public void RemoveAmount(int amount)
    {
        this.amount -= amount;
    }
}
