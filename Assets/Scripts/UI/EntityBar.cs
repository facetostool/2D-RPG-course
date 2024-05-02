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
        Debug.Log("EntityBar Start");
        
        rTransform = GetComponentInChildren<RectTransform>();
        
        UpdateHealthBarValues();
    }

    private void OnEnable()
    {
        entity = GetComponentInParent<Entity>();
        stats = GetComponentInParent<EntityStats>();
        healthSlider = GetComponentInChildren<Slider>();
        
        Debug.Log("EntityBar OnEnable");
        entity.onFliped += FlipBar;
        stats.onHealthChanged += UpdateHealthBarValues;
    }
    
    private void OnDisable()
    {
        entity.onFliped -= FlipBar;
        stats.onHealthChanged -= UpdateHealthBarValues;
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
        healthSlider.maxValue = stats.MaxHealthValue();
        healthSlider.value = stats.currentHealth;
    }
    
}
