using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, ISaveManager
{
    #region Singleton

    public static PlayerManager instance;
    
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    #endregion

    [field: SerializeField] public Player player { get; private set; }
    [SerializeField] private GameObject deadLightPrefab;
    
    [SerializeField] private int currency = 0;
    [SerializeField] private DeadLightController lastDeadLight;

    private void Start()
    {
        player.onDie += OnDie;
    }

    private void Update()
    {
    }

    private void OnDie()
    {
        GameObject deadLight = Instantiate(deadLightPrefab, player.transform.position, Quaternion.identity);
        lastDeadLight = deadLight.GetComponent<DeadLightController>();
        lastDeadLight.Setup(currency);
        currency = 0;
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

    public void SaveData(ref GameData data)
    {
        data.currency = currency;
        
        if (lastDeadLight != null)
        {
            data.lastDeadLightCurrency = lastDeadLight.currency;
            data.lastDeadLightPosition = lastDeadLight.transform.position;
        }
        else
        {
            data.lastDeadLightCurrency = 0;
            data.lastDeadLightPosition = Vector2.zero;
        }
    }

    public void LoadData(GameData data)
    {
        currency = data.currency;
        
        if (data.lastDeadLightCurrency > 0)
        {
            var deadLight = Instantiate(deadLightPrefab, data.lastDeadLightPosition, Quaternion.identity);
            lastDeadLight = deadLight.GetComponent<DeadLightController>();
            lastDeadLight.Setup(data.lastDeadLightCurrency);
        }
    }
}
