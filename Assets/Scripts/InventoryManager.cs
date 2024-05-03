using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    
    public List<InventoryItem> inventoryItems;
    public Dictionary<ItemData, InventoryItem> inventoryItemDict;
    
    [SerializeField] private GameObject inventoryPanel;
    private ItemSlot[] itemSlots;
    
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        inventoryItems = new List<InventoryItem>();
        inventoryItemDict = new Dictionary<ItemData, InventoryItem>();
        
        itemSlots = inventoryPanel.GetComponentsInChildren<ItemSlot>();
    }
    
    public void UpdateUI()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (i < inventoryItems.Count)
            {
                itemSlots[i].SetItem(inventoryItems[i]);
            }
            else
            {
                itemSlots[i].ClearSlot();
            }
        }
    }

    public void AddItem(ItemData item)
    {
        if (inventoryItemDict.ContainsKey(item))
        {
            inventoryItemDict[item].AddAmount(1);
        }
        else
        {
            InventoryItem newItem = new InventoryItem(item);
            inventoryItems.Add(newItem);
            inventoryItemDict.Add(item, newItem);
        }
        
        UpdateUI();
    }
    
    public void RemoveItem(ItemData item)
    {
        if (inventoryItemDict.ContainsKey(item))
        {
            inventoryItemDict[item].RemoveAmount(1);
            if (inventoryItemDict[item].amount <= 0)
            {
                inventoryItems.Remove(inventoryItemDict[item]);
                inventoryItemDict.Remove(item);
            }
        }
        
        UpdateUI();
    }
}
