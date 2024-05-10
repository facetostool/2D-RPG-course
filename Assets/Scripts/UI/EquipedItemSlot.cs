using UnityEngine.EventSystems;

public class EquipedItemSlot : ItemSlot
{
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (inventoryItem == null)
            return;

        InventoryManager.instance.UnequipItem(this);
    }
}
