using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] public ItemTooltip itemTooltip;
    [SerializeField] public StatTooltip statTooltip;
    
    [SerializeField] public GameObject inventoryTab;
    [SerializeField] public GameObject craftTab;
    [SerializeField] public GameObject skillTreeTab;
    [SerializeField] public GameObject optionsTab;
    
    // Start is called before the first frame update
    void Start()
    {   
        SwitchToTab(null);
        itemTooltip.HideTooltip();
        statTooltip.HideTooltip();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            SwitchToTabByKey(inventoryTab);
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchToTabByKey(craftTab);
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            SwitchToTabByKey(skillTreeTab);
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            SwitchToTabByKey(optionsTab);
        }
    }
    
    public void SwitchToTab(GameObject tab)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        
        if (tab != null)
            tab.SetActive(true);
    }

    public void SwitchToTabByKey(GameObject tab)
    {
        if (tab != null && tab.activeSelf)
        {
            tab.SetActive(false);
            return;
        }
        SwitchToTab(tab);
    }
}
