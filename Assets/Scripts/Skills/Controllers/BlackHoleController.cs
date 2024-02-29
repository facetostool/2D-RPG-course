using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleController : MonoBehaviour
{
    public float growSpeed;
    public bool canGrow;
    public float maxSize;
    public List<Transform> enemies = new List<Transform>();
    
    private SpriteRenderer sr;

    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDisappearSpeed;
    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> hotKeys;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        if (canGrow)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(maxSize, maxSize), growSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            foreach (var enemy in enemies)
            {
                int offset = 0;
                int rnd = Random.Range(0, 50);
                if (rnd < 25)
                {
                    offset = 1;
                } else
                {
                    offset = -1;
                }
                GameObject clone = Instantiate(clonePrefab, new Vector3(enemy.transform.position.x + offset*2, enemy.transform.position.y), Quaternion.identity);
                clone.GetComponent<CloneController>().Setup(cloneDisappearSpeed);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy)
        {
            enemy.Freeze();
                
            GameObject hotKey = Instantiate(hotKeyPrefab, enemy.transform.position + new Vector3(0, 2), Quaternion.identity);
            hotKey.GetComponent<HotKeyController>().Setup(GetRandomKey(), gameObject, enemy.transform);
        }
    }
    
    public void AddEnemy(Transform enemy)
    {
        enemies.Add(enemy);
    }
    
    private KeyCode GetRandomKey()
    {
        int randomIndex = UnityEngine.Random.Range(0, hotKeys.Count);
        KeyCode randomKey = hotKeys[randomIndex];
        hotKeys.RemoveAt(randomIndex);

        return randomKey;
    }
}
