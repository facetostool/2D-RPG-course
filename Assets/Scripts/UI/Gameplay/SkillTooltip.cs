using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillDescription;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ShowTooltip(string _name, string _description)
    {
        enabled = true;
        skillName.text = _name;
        skillDescription.text = _description;
    }
    
    public void HideTooltip()
    {
        enabled = false;
    }
}
