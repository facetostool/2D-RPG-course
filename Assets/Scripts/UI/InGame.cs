using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGame : MonoBehaviour
{
    public PlayerStats stats;
    public Slider healthSlider;
    public TextMeshProUGUI currencyText;

    private void Awake()
    {
        UpdateHealthBarValues();
        
        stats.onHealthChanged += UpdateHealthBarValues;
        stats.onStatsChanged += UpdateHealthBarValues;
    }
    
    void UpdateHealthBarValues()
    {
        healthSlider.maxValue = stats.MaxHealthValue();
        healthSlider.value = stats.currentHealth;
    }

    private void Update()
    {
        currencyText.text = PlayerManager.instance.GetCurrency().ToString("#,#");
    }
}
