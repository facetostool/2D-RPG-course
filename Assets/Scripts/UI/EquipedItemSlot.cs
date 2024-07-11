using UnityEngine;
using UnityEngine.EventSystems;

public class EquipedItemSlot : ItemSlot
{
    [SerializeField] public EquipmentType equipmentType;
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (inventoryItem == null || inventoryItem.itemData == null || inventoryItem.itemData.itemType != ItemType.Equipment)
            return;

        InventoryManager.instance.UnequipItem(this);
    }
}
