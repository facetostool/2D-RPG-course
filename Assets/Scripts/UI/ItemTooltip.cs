using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTooltip : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI itemName;
    [SerializeField] private TMPro.TextMeshProUGUI itemType;
    [SerializeField] private TMPro.TextMeshProUGUI itemStats;
    [SerializeField] private int defaultFontSize = 36;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ShowTooltip(ItemDataEquipment itemData)
    {
        gameObject.SetActive(true);
        
        itemName.fontSize = defaultFontSize;
        if (itemData.itemName.Length > 12)
            itemName.fontSize = defaultFontSize*0.75f;
        
        itemName.text = itemData.itemName;
        itemType.text = itemData.equipmentType.ToString();
        itemStats.text = itemData.GetStatsDescription();
        
    }
    
    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}
