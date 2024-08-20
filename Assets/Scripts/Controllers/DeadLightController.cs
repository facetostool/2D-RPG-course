using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadLightController : MonoBehaviour
{
    [SerializeField] public int currency;
    
    [Header("Movement")]
    [SerializeField] private float amplitude;
    [SerializeField] private float speed;
    
    private float startY;
    
    // Start is called before the first frame update
    void Start()
    {
        startY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        var position = transform.position;
        position.y = startY + Mathf.Sin(Time.time * speed) * amplitude;
        transform.position = position;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (other.GetComponent<Player>().IsDead()) return;
        
        PlayerManager.instance.AddCurrency(currency);
        Destroy(gameObject);
    }
    
    public void Setup(int currency)
    {
        this.currency = currency;
    }
}
