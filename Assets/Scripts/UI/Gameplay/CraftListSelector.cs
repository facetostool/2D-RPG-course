using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CraftListSelector : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameObject craftSlotPrefab;
    [SerializeField] private Transform craftSlotParent;
    [SerializeField] private List<ItemDataEquipment> craftItems;
    
    // Start is called before the first frame update
    void SetupList()
    {
        foreach (var recipe in craftSlotParent.GetComponentsInChildren<CraftItemSlot>())
        {
            Destroy(recipe.gameObject);
        }
        
        foreach (var item in craftItems)
        {
            var slot = Instantiate(craftSlotPrefab, craftSlotParent).GetComponent<CraftItemSlot>();
            slot.SetItem(item);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SetupList();
        
        GetComponentInParent<UI>().craftItemWindow.HideContent();
    }
}
