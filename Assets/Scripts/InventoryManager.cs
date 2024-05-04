using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public Player player;
    
    public List<InventoryItem> inventoryItems;
    public Dictionary<ItemData, InventoryItem> inventoryItemDict;
    
    public List<InventoryItem> stashItems;
    public Dictionary<ItemData, InventoryItem> stashItemDict;
    
    public List<InventoryItem> equipmentItems;
    public Dictionary<EquipmentType, InventoryItem> equipmentItemDict;
    
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject stashPanel;
    [SerializeField] private GameObject equipmentPanel;
    
    [SerializeField] private GameObject itemSlotPrefab;
    
    private ItemSlot[] inventorySlots;
    private ItemSlot[] stashSlots;
    private ItemSlot[] equipmentSlots;
    
    private int MAX_INVENTORY_SIZE = 10;
    private int MAX_STASH_SIZE = 20;
    private int MAX_EQUIPMENT_SIZE = 4;
    
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
        player = PlayerManager.instance.player;
        
        inventoryItems = new List<InventoryItem>();
        inventoryItemDict = new Dictionary<ItemData, InventoryItem>();
        
        stashItems = new List<InventoryItem>();
        stashItemDict = new Dictionary<ItemData, InventoryItem>();
        
        equipmentItems = new List<InventoryItem>();
        equipmentItemDict = new Dictionary<EquipmentType, InventoryItem>();
        
        inventorySlots = CreateItemSlots(inventoryPanel, MAX_INVENTORY_SIZE);
        stashSlots = CreateItemSlots(stashPanel, MAX_STASH_SIZE);
        equipmentSlots = CreateItemSlots(equipmentPanel, MAX_EQUIPMENT_SIZE);
    }
    
    public ItemSlot[] CreateItemSlots(GameObject panel, int slotAmount)
    {
        ItemSlot[] slots = new ItemSlot[slotAmount];
        for (int i = 0; i < slotAmount; i++)
        {
            GameObject newSlot = Instantiate(itemSlotPrefab, panel.transform);
            slots[i] = newSlot.GetComponent<ItemSlot>();
        }

        return slots;
    }
    
    public void EquipItem(ItemSlot inventorySlot)
    {
        ItemDataEquipment item = (ItemDataEquipment)inventorySlot.inventoryItem.itemData;
        if (equipmentItemDict.ContainsKey(item.equipmentType))
        {
            ItemSlot slot = FindEquipmentSlotFor(item.equipmentType);
            
           if (slot == null)
           {
               throw new Exception("No slot found to replace");
           } 
           
           InventoryItem newEquipmentItem = new InventoryItem(item);
           InventoryItem inventoryItemToReplace = slot.inventoryItem;
           slot.SetItem(newEquipmentItem);
           
           equipmentItems.Add(newEquipmentItem);
           item.AddModifiers(player.stats);
           equipmentItems.Remove(inventoryItemToReplace);
           var oldEquipment = (ItemDataEquipment)inventoryItemToReplace.itemData;
           oldEquipment.RemoveModifiers(player.stats);
           
           RemoveInventoryItem(newEquipmentItem.itemData);
           AddInventoryItem(inventoryItemToReplace.itemData);
        }
        else
        {
            InventoryItem newItem = new InventoryItem(item);
            equipmentItems.Add(newItem);
            item.AddModifiers(player.stats);
            equipmentItemDict.Add(item.equipmentType, newItem);
            UpdateUI(equipmentSlots, equipmentItems);
            
            RemoveInventoryItem(item);
        }
    }

    private ItemSlot FindEquipmentSlotFor(EquipmentType type)
    {
        for (int i = 0; i < equipmentSlots.Length; i++)
        {
            ItemDataEquipment equipmentItem = (ItemDataEquipment)equipmentSlots[i].inventoryItem.itemData;
            if (equipmentItem.equipmentType == type)
            {
                return equipmentSlots[i];
            }
        }

        return null;
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
            if (inventoryItems.Count >= inventorySlots.Length)
            {
                Debug.Log("Inventory is full");
                return;
            }
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
