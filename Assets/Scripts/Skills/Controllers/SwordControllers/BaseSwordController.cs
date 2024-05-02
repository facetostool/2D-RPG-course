using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSwordController : MonoBehaviour
{
    protected Player player;
    protected Animator animator;
    protected Rigidbody2D rb;
    protected CircleCollider2D circleCollider2D;
    
    protected bool isSpinning;
    protected bool isReturning;
    protected float returnSpeed;
    protected float freezeTime;
    
    protected static readonly int Spin = Animator.StringToHash("Spin");
    
    protected void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        circleCollider2D = GetComponent<CircleCollider2D>();
        player = PlayerManager.instance.player;
        
        isReturning = false;
        isSpinning = true;
    }

    protected virtual void Start()
    {
        
    }

    
    protected virtual void Update()
    {
        if (isSpinning && !isReturning)
            transform.right = rb.velocity;
        
        if (Input.GetKey(KeyCode.Mouse1))
        {
            isReturning = true;
            transform.parent = null;
            circleCollider2D.enabled = false;
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        
        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(
                transform.position, 
                player.transform.position,
                returnSpeed * Time.deltaTime
            );
            
            if (transform.position == player.transform.position)
            {
                player.ClearSword();
            }
        }
    }
    
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {

    }
    
    protected void SetupBase(Vector2 _throwForce, float _gravityScale, float _returnSpeed, float _freezeTime)
    {
        enabled = true;
        
        rb.velocity = new Vector2(_throwForce.x, _throwForce.y);
        rb.gravityScale = _gravityScale;
        player.SetSword(gameObject);
        returnSpeed = _returnSpeed;
        freezeTime = _freezeTime;
        
        animator.SetBool("Spin", true);
    }
    
    protected void StuckSword(Collider2D other)
    {
        animator.SetBool(Spin, false);
        transform.parent = other.transform;
        circleCollider2D.enabled = false;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        isSpinning = false;
    }

    protected void RegisterDamage(Enemy enemy)
    {
        enemy.DamageEffect();
        enemy.StartCoroutine(nameof(Enemy.FreezeFor), freezeTime);
    }
}
