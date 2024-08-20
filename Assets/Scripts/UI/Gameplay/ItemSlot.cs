using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    protected UI ui;
    
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemAmount;
    [SerializeField] public InventoryItem inventoryItem;

    void Start()
    {
        ui = GetComponentInParent<UI>();
    }
    
    public virtual void Awake()
    {
        itemImage = GetComponent<Image>();
        itemAmount = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetItem(InventoryItem item)
    {
        if (!itemImage)
            return;
        
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
        if (!itemImage)
            return;
        
        itemImage.color = Color.clear;
        inventoryItem = null;
        GetComponent<Image>().sprite = null;
        itemAmount.text = "";
    }
    
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (inventoryItem == null)
            return;
        
        ui.itemTooltip.HideTooltip();
        
        if (Input.GetKey(KeyCode.LeftControl))
        {
            InventoryManager.instance.RemoveItem(inventoryItem.itemData);
            return;
        }
        
        ItemData itemData = inventoryItem.itemData;
        if (itemData != null && itemData.itemType == ItemType.Equipment)
        {
            InventoryManager.instance.EquipItem(this.inventoryItem);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (inventoryItem == null || inventoryItem.itemData == null || inventoryItem.itemData.itemType != ItemType.Equipment)
            return;
        
        ui.itemTooltip.ShowTooltip(inventoryItem.itemData as ItemDataEquipment);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (inventoryItem == null || inventoryItem.itemData == null || inventoryItem.itemData.itemType != ItemType.Equipment)
            return;
        
        ui.itemTooltip.HideTooltip();
    }
}
