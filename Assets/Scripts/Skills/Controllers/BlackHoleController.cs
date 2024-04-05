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
    private bool isHotkeyRemoved;
    
    private float attackTimer;
    private float skillTimer;
    private int currentAttackTarget;
    private List<Transform> enemiesToAttack = new List<Transform>();
    private List<Enemy> freezedEnemies = new List<Enemy>();
    
    private SpriteRenderer sr;

    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDisappearSpeed;
    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> hotKeys;
    
    private List<GameObject> hotKeyObjects = new List<GameObject>();

    public void Setup( float _growSpeed, float _shrinkSpeed, float _maxSize, float _attackCooldown, float _attackNumber, float _skillTimer)
    {
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        maxSize = _maxSize;
        attackCooldown = _attackCooldown;
        attackNumber = _attackNumber;
        skillTimer = _skillTimer;
        
        canGrow = true;
    }
    
    void Start()
    {
    }
    
    void Update()
    {
        skillTimer -= Time.deltaTime;
        
        if (canGrow)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(maxSize, maxSize), growSpeed * Time.deltaTime);
        }
        
        if (canShrink)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(-1, -1), shrinkSpeed * Time.deltaTime);
            if (transform.localScale.x <= 0.1f)
            {
                ExitBlackHole();
            }
        }

        if (Input.GetKeyDown(KeyCode.R) || skillTimer <= 0)
        {
            if (!isHotkeyRemoved)
                ClearHotkeys();
            isReleased = true;
            canGrow = false;
        }
        
        attackTimer -= Time.deltaTime;

        if (isReleased && enemiesToAttack.Count <= 0)
        {
            StartToShrink();
        }
        
        if (isReleased && attackTimer <= 0 && enemiesToAttack.Count > 0 && attackNumber > 0)
        {
            PlayerManager.instance.player.MakeTransparent();
            AttackEnemy();
            if (attackNumber <= 0)
            {
                Invoke(nameof(StartToShrink), 1f);
            }
        }
    }

    private void ExitBlackHole()
    {
        PlayerManager.instance.player.ExitUltimate();
        foreach (var enemy in freezedEnemies)
        {
            enemy.Unfreeze();
        }
        
        Destroy(gameObject);
    }

    private void ClearHotkeys()
    {
        foreach (var hotKey in hotKeyObjects)
        {
            Destroy(hotKey);
        }
        isHotkeyRemoved = true;
    }

    private void StartToShrink()
    {
        canShrink = true;
    }

    private void AttackEnemy()
    {
        attackTimer = attackCooldown;
        Transform enemy = enemiesToAttack[currentAttackTarget].transform;

        var enemyPosition = enemy.transform.position;
        SkillManager.instance.clone.Use(new Vector3(enemyPosition.x + RandomOffset()*1f, enemyPosition.y), enemy);
        currentAttackTarget++;
        attackNumber--;
        if (currentAttackTarget >= enemiesToAttack.Count)
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
        if (isReleased)
            return;
        
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy)
        {
            enemy.Freeze();
            freezedEnemies.Add(enemy);
                
            GameObject hotKey = Instantiate(hotKeyPrefab, enemy.transform.position + new Vector3(0, 2), Quaternion.identity);
            hotKey.GetComponent<HotKeyController>().Setup(GetRandomKey(), gameObject, enemy.transform);
            hotKeyObjects.Add(hotKey);
        }
    }
    
    public void AddEnemy(Transform enemy)
    {
        enemiesToAttack.Add(enemy);
    }
    
    private KeyCode GetRandomKey()
    {
        int randomIndex = Random.Range(0, hotKeys.Count);
        KeyCode randomKey = hotKeys[randomIndex];
        hotKeys.RemoveAt(randomIndex);

        return randomKey;
    }
}
