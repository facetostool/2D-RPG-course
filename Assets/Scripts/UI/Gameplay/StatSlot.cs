using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StatSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private UI ui;
    
    [SerializeField] private StatType statType;
    [SerializeField] private TMPro.TextMeshProUGUI statValue;
    [SerializeField] private TMPro.TextMeshProUGUI statName;
    [SerializeField] private string statNameString;
    [TextArea(3, 10)]
    [SerializeField] private string statDescription;

    private PlayerStats playerStats;
    private void OnValidate()
    {
        gameObject.name = statNameString;
        
        if (statName != null)
            statName.text = statNameString;
    }

    // Start is called before the first frame update
    void Start()
    {
        ui = GetComponentInParent<UI>();
        
        playerStats = PlayerManager.instance.player.stats as PlayerStats;
        if (playerStats != null) playerStats.onStatsChanged += UpdateUI;
        UpdateUI();
    }
    
    void UpdateUI()
    {
        statValue.text = statType switch
        {
            StatType.evasion => playerStats.GetEvadeChance().ToString(),
            StatType.magicResistance => playerStats.GetMagicResistance().ToString(),
            StatType.damage => playerStats.GetDamage().ToString(),
            StatType.critChance => playerStats.GetCritChance().ToString(),
            StatType.critPower => playerStats.GetCritPower().ToString(),
            StatType.maxHealth => playerStats.MaxHealthValue().ToString(),
            _ => playerStats.GetStat(statType).Value().ToString()
        };
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.statTooltip.ShowTooltip(statDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.statTooltip.HideTooltip();
    }
}
