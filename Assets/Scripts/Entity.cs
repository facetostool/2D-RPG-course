using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Animator animator { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public EntityFX fx {get; private set;}
    public SpriteRenderer sr { get; private set; }
    public EntityStats stats { get; private set; }
    
    [SerializeField] public float moveSpeed;
    public float faceDir = 1;
    
    [Header("Collisions")]
    [SerializeField] public Transform groundCheck;
    [SerializeField] public float groundCheckDistance;
    [SerializeField] public LayerMask whatIsGround;
    [SerializeField] public Transform wallCheck;
    [SerializeField] public float wallCheckDistance;

    [Header("Attack collisions")]
    [SerializeField] public Transform attackCheck;
    [SerializeField] public float attackCheckRadius;
    
    private float defaultAlpha;

    public Action onFliped;
    
    protected virtual void Awake()
    {
        
    }

    protected virtual void Start()
    {
        fx = GetComponentInChildren<EntityFX>();
        sr = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<EntityStats>();
        
        defaultAlpha = sr.color.a;
    }
    
    protected virtual void Update()
    {
        
    }
    
    public virtual void SlowBy(float slowAmount, float slowTime)
    {
        
    }

    protected virtual void ReturnDefaultSpeed()
    {
        
    }
    
    public void Flip()
    {
        faceDir = -faceDir;
        transform.Rotate(0, 180, 0);
        onFliped?.Invoke();
    }
    
    public virtual void SetVelocity(float x, float y)
    {
        rb.velocity = new Vector2(x, y);
    }

    public virtual void DamageEffect() => fx.StartCoroutine("Flash");
    
    public void MakeVisible()
    {
        Color tmp = sr.color;
        tmp.a = defaultAlpha;
        sr.color = tmp;
    }

    #region Collisions

    public virtual void OnDrawGizmos()
             {
                 Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
                 Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance*faceDir, wallCheck.position.y));
                 Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
             }
         
             public bool IsGroundDetected()
             {
                 return Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
             }
         
             public bool IsWallDetected()
             {
                 return Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance*faceDir, whatIsGround);
             }

    #endregion
}
