using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BlackHoleController : MonoBehaviour
{
    private float growSpeed;
    private float shrinkSpeed;
    private float maxSize;
    private float attackCooldown;
    private float attackNumber;
    
    private bool canGrow;
    private bool canShrink;
    private bool isReleased;
    
    private float attackTimer;
    private int currentAttackTarget;
    private List<Transform> enemies = new List<Transform>();
    
    private SpriteRenderer sr;

    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDisappearSpeed;
    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> hotKeys;

    public void Setup( float _growSpeed, float _shrinkSpeed, float _maxSize, float _attackCooldown, float _attackNumber)
    {
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        maxSize = _maxSize;
        attackCooldown = _attackCooldown;
        attackNumber = _attackNumber;
        
        canGrow = true;
    }
    
    void Start()
    {
    }
    
    void Update()
    {
        if (canGrow)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(maxSize, maxSize), growSpeed * Time.deltaTime);
        }
        
        if (canShrink)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(-1, -1), shrinkSpeed * Time.deltaTime);
            if (transform.localScale.x <= 0.1f)
            {
                PlayerManager.instance.player.MakeVisible();
                Destroy(gameObject);
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            isReleased = true;
            canGrow = false;
        }
        
        attackTimer -= Time.deltaTime;

        if (isReleased && enemies.Count < 0)
        {
            StartToShrink();
        }
        
        if (isReleased && attackTimer <= 0 && enemies.Count > 0 && attackNumber > 0)
        {
            PlayerManager.instance.player.MakeTransparent();
            AttackEnemy();
            if (attackNumber <= 0)
            {
                Invoke(nameof(StartToShrink), 1f);
            }
        }
    }

    private void StartToShrink()
    {
        canShrink = true;
    }

    private void AttackEnemy()
    {
        attackTimer = attackCooldown;
        Transform enemy = enemies[currentAttackTarget];

        var enemyPosition = enemy.transform.position;
        GameObject clone = Instantiate(clonePrefab, new Vector3(enemyPosition.x + RandomOffset()*1.5f, enemyPosition.y), Quaternion.identity);
        clone.GetComponent<CloneController>().Setup(cloneDisappearSpeed);
        currentAttackTarget++;
        attackNumber--;
        if (currentAttackTarget >= enemies.Count)
        {
            currentAttackTarget = 0;
        }
    }

    private static int RandomOffset()
    {
        int offset;
        int rnd = Random.Range(0, 50);
        if (rnd < 25)
        {
            offset = 1;
        } else
        {
            offset = -1;
        }

        return offset;
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
