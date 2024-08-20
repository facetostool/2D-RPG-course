using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObjectTrigger : MonoBehaviour
{
    [SerializeField] private Vector2 velocity;
    private ItemObject item;
    
    void Start()
    {
        item = GetComponentInParent<ItemObject>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;
        
        if (other.GetComponent<Player>().IsDead())
            return;
        
        item.PickupItem();
    }
}
