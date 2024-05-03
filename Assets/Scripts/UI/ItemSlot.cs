using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemAmount;
    [SerializeField] private InventoryItem inventoryItem;

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
}
