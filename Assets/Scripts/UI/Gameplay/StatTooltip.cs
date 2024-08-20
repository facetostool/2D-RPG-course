using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatTooltip : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI statDescription;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ShowTooltip(string description)
    {
        gameObject.SetActive(true);
        statDescription.text = description;
    }
    
    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}
