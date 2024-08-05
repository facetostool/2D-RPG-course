using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public Player player;

    private float lastTimeFlaskUsed;
    private float flaskCooldown;
    
    private float lastTimeArmorUsed;
    private float armorCooldown;
    
    [SerializeField] private SkillImageCooldown flaskImageCooldown;

    [SerializeField] private List<ItemData> defaultInventoryItems = new List<ItemData>();

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
    private EquipedItemSlot[] equipmentSlots;

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

        inventorySlots = inventoryPanel.GetComponentsInChildren<ItemSlot>();
        stashSlots = stashPanel.GetComponentsInChildren<ItemSlot>();
        equipmentSlots = equipmentPanel.GetComponentsInChildren<EquipedItemSlot>();

        foreach (var item in defaultInventoryItems)
        {
            AddItem(item);
        }
    }

    public void EquipItem(ItemSlot inventorySlot)
    {
        ItemDataEquipment item = (ItemDataEquipment)inventorySlot.inventoryItem.itemData;
        if (equipmentItemDict.ContainsKey(item.equipmentType))
        {
            ItemSlot slot = FindEquipmentSlotFor(item.equipmentType);
            
            InventoryItem newEquipmentItem = new InventoryItem(item);
            InventoryItem inventoryItemToReplace = slot.inventoryItem;
            slot.SetItem(newEquipmentItem);

            equipmentItems.Add(newEquipmentItem);
            equipmentItemDict[item.equipmentType] = newEquipmentItem;
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
            equipmentItemDict.Add(item.equipmentType, newItem);
            item.AddModifiers(player.stats);
            
            ItemSlot slot = FindEquipmentSlotFor(item.equipmentType);
            slot.SetItem(newItem);

            RemoveInventoryItem(item);
        }
    }

    public void UnequipItem(ItemSlot equipmentSlot)
    {
        ItemDataEquipment item = (ItemDataEquipment)equipmentSlot.inventoryItem.itemData;
        item.RemoveModifiers(player.stats);
        equipmentItems.Remove(equipmentSlot.inventoryItem);
        equipmentItemDict.Remove(item.equipmentType);
        equipmentSlot.ClearSlot();

        AddInventoryItem(item);
    }

    public void UnequipItem(ItemData item)
    {
        if (equipmentItemDict.TryGetValue(((ItemDataEquipment)item).equipmentType, out var inventoryItem))
        {
            UnequipItem(FindEquipmentSlotFor(((ItemDataEquipment)item).equipmentType));
        }
    }

    private ItemSlot FindEquipmentSlotFor(EquipmentType type)
    {
        return equipmentSlots.FirstOrDefault(slot => slot.equipmentType == type);
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

    public bool CanAddItem(ItemData item)
    {
        switch (item.itemType)
        {
            case ItemType.Material:
                return true;
            case ItemType.Equipment:
                return inventoryItems.Count < inventorySlots.Length;
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

    public bool CanCraft(ItemDataEquipment item)
    {
        if (item!.craftMaterials.Count == 0)
            return false;

        foreach (var material in item.craftMaterials)
        {
            if (!stashItemDict.TryGetValue(material.itemData, out var stashItem))
                return false;

            if (stashItem.amount < material.amount)
                return false;
        }

        return true;
    }

    public void CraftItem(ItemDataEquipment item)
    {
        if (item?.craftMaterials == null) return;

        foreach (var material in item.craftMaterials)
        {
            RemoveStashItem(material.itemData);
        }

        AddInventoryItem(item);
    }

    public bool CanUseFlask()
    {
        if (!equipmentItemDict.TryGetValue(EquipmentType.Flask, out _)) return false;
        return Time.time - lastTimeFlaskUsed > flaskCooldown;
    }
    
    public void UseFlask()
    {
        if (equipmentItemDict.TryGetValue(EquipmentType.Flask, out var flaskItem))
        {
            var flask = (ItemDataEquipment)flaskItem.itemData;
            flask.ApplyEffects(null);
            lastTimeFlaskUsed = Time.time;
            flaskCooldown = flask.cooldown;
            
            if (flaskImageCooldown)
                flaskImageCooldown.StartCooldown(flask.cooldown);
        }
    }
    
    public bool CanUseArmor()
    {
        if (!equipmentItemDict.TryGetValue(EquipmentType.Armor, out _)) return false;
        return Time.time - lastTimeArmorUsed > armorCooldown;
    }
    
    public void UseArmor()
    {
        if (equipmentItemDict.TryGetValue(EquipmentType.Armor, out var armorItem))
        {
            var armor = (ItemDataEquipment)armorItem.itemData;
            armor.ApplyEffects(player.transform);
            lastTimeArmorUsed = Time.time;
            armorCooldown = armor.cooldown;
        }
    }
}