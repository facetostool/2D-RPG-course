using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemObject : MonoBehaviour
{
    [SerializeField] ItemData itemData;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void SetupVisuals()
    {
        if (itemData != null)
        {
            GetComponent<SpriteRenderer>().sprite = itemData.icon;
            gameObject.name = itemData.itemType + ": " +  itemData.itemName;
        }
    }
    
    public void SetupItem(ItemData item, Vector2 velocity)
    { 
        itemData = item;
        rb.velocity = velocity;
        
        SetupVisuals();
    }

    public void PickupItem()
    {
        if (!InventoryManager.instance.CanAddItem(itemData))
        {
            rb.velocity = new Vector2(0, 5);
            return;
        }
        InventoryManager.instance.AddItem(itemData);
        Destroy(gameObject);
    }
}
