using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private GameObject itemObjectPrefab;
    [SerializeField] private List<ItemData> itemsToDrop = new List<ItemData>();
    [SerializeField] private int itemsToDropAmount = 1;
    
    public virtual void GenerateDrop()
    {
        List<ItemData> randomItemsToDrop = new List<ItemData>();
        foreach (var item in itemsToDrop)
        {
            if (Random.Range(0, 100) < item.dropChance)
            {
                randomItemsToDrop.Add(item);
            }
        }
        
        for (int i = 0; i < itemsToDropAmount; i++)
        {
            if (randomItemsToDrop.Count > 0)
            {
                DropItem(randomItemsToDrop[Random.Range(0, randomItemsToDrop.Count)]);
            }
        }
    }

    
    protected void DropItem(ItemData item)
    {
        GameObject itemObject = Instantiate(itemObjectPrefab, transform.position, Quaternion.identity);
        itemObject.GetComponent<ItemObject>().SetupItem(item, new Vector2(Random.Range(-5f, 5f), Random.Range(-15f, 15f)));
    }
}
