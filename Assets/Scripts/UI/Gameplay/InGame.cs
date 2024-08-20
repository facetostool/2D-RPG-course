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

    [Header("Souls")]
    [SerializeField] public TextMeshProUGUI currencyText;
    [SerializeField] private int currencyAmount;
    [SerializeField] private int currencyRate;
    
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
        if (currencyAmount < PlayerManager.instance.GetCurrency())
        {
            currencyAmount += Mathf.RoundToInt(currencyRate * Time.deltaTime);
        }
        else
        {
            currencyAmount = PlayerManager.instance.GetCurrency();
        }
        
        currencyText.text = currencyAmount.ToString("#,#;#,#;0");
    }
}
