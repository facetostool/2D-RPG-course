using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftItemWindow : MonoBehaviour
{
    [SerializeField] private GameObject[] materials;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private GameObject content;
    
    private ItemDataEquipment item;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Setup(ItemDataEquipment _item)
    {
        ShowContent();
        
        item = _item;
        
        foreach (var material in materials)
        {
            material.SetActive(false);
        }
        
        icon.sprite = item.icon;
        itemName.text = item.itemName;
        itemDescription.text = item.GetStatsDescription();
        
        for (int i = 0; i < item.craftMaterials.Count; i++)
        {
            var amount = materials[i].GetComponentInChildren<TextMeshProUGUI>();
            materials[i].SetActive(true);
            var image = materials[i].GetComponent<Image>();
            image.sprite = item.craftMaterials[i].itemData.icon;
            amount.text = item.craftMaterials[i].amount.ToString();
        }
    }
    
    public void CraftItem()
    {
        if (!InventoryManager.instance.CanCraft(item))
        {
            Debug.Log("Cannot craft item");
            SoundManager.instance.PlaySFX(SfxEffect.CraftFail);
            return;  
        }
        
        SoundManager.instance.PlaySFX(SfxEffect.SuccessCraft);
        InventoryManager.instance.CraftItem(item);
    }
    
    public void ShowContent()
    {
        content.SetActive(true);
    }
    
    public void HideContent()
    {
        content.SetActive(false);
    }
}
