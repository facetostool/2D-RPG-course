using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntityBar : MonoBehaviour
{
    public EntityStats stats;
    public Entity entity;
    public RectTransform rTransform;
    public Slider healthSlider;
    
    void Start()
    {
        rTransform = GetComponentInChildren<RectTransform>();
        
        UpdateHealthBarValues();
    }

    private void OnEnable()
    {
        entity = GetComponentInParent<Entity>();
        stats = GetComponentInParent<EntityStats>();
        healthSlider = GetComponentInChildren<Slider>();
        
        entity.onFliped += FlipBar;
        stats.onHealthChanged += UpdateHealthBarValues;
        stats.onStatsChanged += UpdateHealthBarValues;
    }
    
    private void OnDisable()
    {
        entity.onFliped -= FlipBar;
        stats.onHealthChanged -= UpdateHealthBarValues;
        stats.onStatsChanged -= UpdateHealthBarValues;
    }

    void FlipBar()
    {
        rTransform.Rotate(0, 180, 0);
    }
    
    void Update()
    {
        
    }
    
    void UpdateHealthBarValues()
    {
        if (stats.currentHealth <= 0)
        {
            rTransform.gameObject.SetActive(false);
        }
        healthSlider.maxValue = stats.MaxHealthValue();
        healthSlider.value = stats.currentHealth;
    }
}
