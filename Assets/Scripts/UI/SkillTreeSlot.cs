using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillTreeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private UI ui;
    
    [SerializeField] private int price;
    [SerializeField] private string skillName;
    [TextArea(3, 5)]
    [SerializeField] private string skillDescription;

    [SerializeField] private List<SkillTreeSlot> requiredUnlocked;
    [SerializeField] private List<SkillTreeSlot> requiredNotUnlocked;
    
    [SerializeField] private Color defaultColor;
    
    public bool unlocked;
    public Action onUnlock;

    private void OnValidate()
    {
        gameObject.name = "Skill: " + skillName;
    }

    // Start is called before the first frame update
    void Start()
    {
        ui = GetComponentInParent<UI>();

        GetComponent<Image>().color = defaultColor;
        
        // add lister to button
        GetComponent<Button>().onClick.AddListener(Unlock);
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillTooltip.ShowTooltip(skillName, skillDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillTooltip.HideTooltip();
    }

    private void Unlock()
    {
        if (!CanUnlock()) return;
        
        PlayerManager.instance.RemoveCurrency(price);
        unlocked = true;
        GetComponent<Image>().color = Color.white;
        onUnlock?.Invoke();
    }

    private bool CanUnlock()
    {
        if (unlocked)
            return false;

        foreach (var skill in requiredUnlocked)
        {
            if (!skill.unlocked)
                return false;
        }

        foreach (var skill in requiredNotUnlocked)
        {
            if (skill.unlocked)
                return false;
        }

        if (!PlayerManager.instance.CanAfford(price))
        {
            Debug.Log("Not enough currency!");
            return false;
        }

        return true;
    }
}
