using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] ItemData itemData;

    private void OnValidate()
    {
        if (itemData != null)
        {
            GetComponent<SpriteRenderer>().sprite = itemData.icon;
            gameObject.name = "Item: " +  itemData.itemName;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            InventoryManager.instance.AddItem(itemData);
            Destroy(gameObject);
        }
    }
}
