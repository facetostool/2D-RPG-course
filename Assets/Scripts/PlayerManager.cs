using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    [field: SerializeField] public Player player { get; private set; }
    
    [SerializeField] private int currency;
    
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    
    public void AddCurrency(int amount)
    {
        currency += amount;
    }
    
    public void RemoveCurrency(int amount)
    {
        currency -= amount;
    }
    
    public int GetCurrency()
    {
        return currency;
    }
    
    public bool CanAfford(int amount)
    {
        return currency >= amount;
    }
}
