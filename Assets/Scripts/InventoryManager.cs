using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    
    public List<InventoryItem> inventoryItems;
    public Dictionary<ItemData, InventoryItem> inventoryItemDict;
    
    public List<InventoryItem> stashItems;
    public Dictionary<ItemData, InventoryItem> stashItemDict;
    
    public List<InventoryItem> equipmentItems;
    public Dictionary<EquipmentType, InventoryItem> equipmentItemDict;
    
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject stashPanel;
    [SerializeField] private GameObject equipmentPanel;
    
    private ItemSlot[] inventorySlots;
    private ItemSlot[] stashSlots;
    private ItemSlot[] equipmentSlots;
    
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
        
        stashItems = new List<InventoryItem>();
        stashItemDict = new Dictionary<ItemData, InventoryItem>();
        
        equipmentItems = new List<InventoryItem>();
        equipmentItemDict = new Dictionary<EquipmentType, InventoryItem>();
        
        inventorySlots = inventoryPanel.GetComponentsInChildren<ItemSlot>();
        stashSlots = stashPanel.GetComponentsInChildren<ItemSlot>();
        equipmentSlots = equipmentPanel.GetComponentsInChildren<ItemSlot>();
    }
    
    public void EquipItem(ItemSlot inventorySlot)
    {
        ItemDataEquipment item = (ItemDataEquipment)inventorySlot.inventoryItem.itemData;
        if (equipmentItemDict.ContainsKey(item.equipmentType))
        {
           ItemSlot slot = null;
           for (int i = 0; i < equipmentSlots.Length; i++)
           {
               ItemDataEquipment equipmentItem = (ItemDataEquipment)equipmentSlots[i].inventoryItem.itemData;
               if (equipmentItem.equipmentType == item.equipmentType)
               {
                   slot = equipmentSlots[i];
                   break;
               }
           }

           if (slot == null)
           {
               throw new Exception("No slot found to replace");
           } 
           
           InventoryItem newEquipmentItem = new InventoryItem(item);
           InventoryItem inventoryItemToReplace = slot.inventoryItem;
           slot.SetItem(newEquipmentItem);
           
           equipmentItems.Add(newEquipmentItem);
           equipmentItems.Remove(inventoryItemToReplace);
           
           RemoveInventoryItem(newEquipmentItem.itemData);
           AddInventoryItem(inventoryItemToReplace.itemData);
        }
        else
        {
            InventoryItem newItem = new InventoryItem(item);
            equipmentItems.Add(newItem);
            equipmentItemDict.Add(item.equipmentType, newItem);
            UpdateUI(equipmentSlots, equipmentItems);
            
            
            RemoveInventoryItem(item);
        }
    }
    
    public void UpdateUI(ItemSlot[] slots, List<InventoryItem> items)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < items.Count)
            {
                slots[i].SetItem(items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }

    public void AddItem(ItemData item)
    {
       switch (item.itemType)
       {
           case ItemType.Material:
               AddStashItem(item);
               break;
           case ItemType.Equipment:
               AddInventoryItem(item);
               break;
           default:
               throw new ArgumentOutOfRangeException();
       }
    }

    private void AddInventoryItem(ItemData item)
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

        UpdateUI(inventorySlots, inventoryItems);
    }
    
    private void AddStashItem(ItemData item)
    {
        if (stashItemDict.ContainsKey(item))
        {
            stashItemDict[item].AddAmount(1);
        }
        else
        {
            InventoryItem newItem = new InventoryItem(item);
            stashItems.Add(newItem);
            stashItemDict.Add(item, newItem);
        }
        
        UpdateUI(stashSlots, stashItems);
    }

    public void RemoveItem(ItemData item)
    {
        switch (item.itemType)
        {
            case ItemType.Material:
                RemoveStashItem(item);
                break;
            case ItemType.Equipment:
                RemoveInventoryItem(item);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void RemoveInventoryItem(ItemData item)
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

        UpdateUI(inventorySlots, inventoryItems);
    }
    
    private void RemoveStashItem(ItemData item)
    {
        if (stashItemDict.ContainsKey(item))
        {
            stashItemDict[item].RemoveAmount(1);
            if (stashItemDict[item].amount <= 0)
            {
                stashItems.Remove(stashItemDict[item]);
                stashItemDict.Remove(item);
            }
        }

        UpdateUI(stashSlots, stashItems);
    }
}
