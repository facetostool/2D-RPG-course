using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CraftItemSlot : ItemSlot
{
    public override void Awake()
    {
        base.Awake();
        SetItem(inventoryItem);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (inventoryItem == null)
            return;

        if (!InventoryManager.instance.CanCraft(this))
        {
            Debug.Log("Cannot craft item");
            return;
        }
        InventoryManager.instance.CraftItem(this);
    }
}