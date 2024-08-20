using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftItemSlot : MonoBehaviour, IPointerDownHandler
{
    private UI ui;
    
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemText;
    [SerializeField] public ItemDataEquipment data;

    void Start()
    {
        ui = GetComponentInParent<UI>();
    }
    
    public void SetItem(ItemDataEquipment _item)
    {
        if (!_item)
            return;
        
        data = _item;
        itemImage.color = Color.white;
        itemImage.sprite = _item.icon;
        itemText.text = _item.itemName;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ui.craftItemWindow.Setup(data);
    }
}