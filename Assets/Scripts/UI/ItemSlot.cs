using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerDownHandler 
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemAmount;
    [SerializeField] public InventoryItem inventoryItem;

    private void Start()
    {
        itemImage = GetComponent<Image>();
        itemAmount = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetItem(InventoryItem item)
    {
        inventoryItem = item;
        itemImage.color = Color.white;
        itemImage.sprite = inventoryItem.itemData.icon;
        
        if (inventoryItem.amount == 1)
        {
            itemAmount.text = "";
            return;
        }
        itemAmount.text = inventoryItem.amount.ToString();
    }
    
    public void ClearSlot()
    {
        itemImage.color = Color.clear;
        inventoryItem = null;
        GetComponent<Image>().sprite = null;
        itemAmount.text = "";
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if (inventoryItem == null)
            return;
        
        ItemData itemData = inventoryItem.itemData;
        if (itemData != null && itemData.itemType == ItemType.Equipment)
        {
            InventoryManager.instance.EquipItem(this);
        }
    }
}
