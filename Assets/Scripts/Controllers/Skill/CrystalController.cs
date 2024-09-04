using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CrystalController : MonoBehaviour
{
    private CircleCollider2D cc;
    private Animator anim;
    private Player player;
    
    private float selfDestroyTimer;
    private bool canExplode;
    private static readonly int ExplodeTrigger = Animator.StringToHash("Explode");

    private bool canGrow;
    private float growSpeed = 3;
    
    private bool canMove;
    private float moveSpeed;
    private Transform closestEnemy;
    
    public void Setup(float _selfDestroyTime, bool _canExplode, bool _canMove, float _moveSpeed, Transform _closestEnemy)
    {
        selfDestroyTimer = _selfDestroyTime;
        canExplode = _canExplode;
        canMove = _canMove;
        moveSpeed = _moveSpeed;
        closestEnemy = _closestEnemy;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        player = PlayerManager.instance.player;
        cc = GetComponent<CircleCollider2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        selfDestroyTimer -= Time.deltaTime;

        if (canMove && closestEnemy)
        {
            transform.position = Vector3.MoveTowards(transform.position, closestEnemy.position, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, closestEnemy.position) < 1f)
            {
                canMove = false;
                Explode();
            }
        }
        
        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3,3), growSpeed * Time.deltaTime);
        }
        
        if (selfDestroyTimer <= 0)
        {
            if (canExplode)
            {
                Explode();
                return;
            }
            SelfDestroy();
        }
    }
    
    public void Explode()
    {
        anim.SetTrigger(ExplodeTrigger);
        canGrow = true;
    }
    
    public void SelfDestroy()
    {
        Destroy(gameObject);
    }

    public void AnimationDmgTrigger()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, cc.radius);
        foreach (var hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy)
            {
                player.stats.DoMagicDamage(enemy.stats, transform);
                
                if (!InventoryManager.instance.equipmentItemDict.TryGetValue(EquipmentType.Amulet, out var amulet))
                    continue;
                if (amulet == null) continue;
                var itemData = amulet.itemData as ItemDataEquipment;
                itemData?.ApplyEffects(enemy.transform);
            }
        }
    }
}
