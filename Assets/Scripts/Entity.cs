using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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
    
    [Header("Knocked info")]
    [SerializeField] public Vector2 knockedForce;
    [SerializeField] public float knockedDuration;
    [FormerlySerializedAs("knockDirection")] public float knockedDirection;
    public bool isKnocked;
    
    private float defaultAlpha;

    public Action onFliped;
    
    protected virtual void Awake()
    {
       
    }

    protected virtual void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<EntityStats>();
        fx = GetComponentInChildren<EntityFX>();
        
        defaultAlpha = sr.color.a;
    }
    
    protected virtual void Update()
    {
        
    }
    
    public bool IsDead()
    {
        return stats.IsDead();
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

    public virtual void DamageEffect(int dmg, bool isCrit)
    {
        fx.StartCoroutine("Flash");
        fx.OnHit();
    }

    public void MakeVisible()
    {
        Color tmp = sr.color;
        tmp.a = defaultAlpha;
        sr.color = tmp;
    }

    #region Collisions
    
    public float ScaledAttackRadius()
    {
        return attackCheckRadius * transform.localScale.x;
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance*faceDir, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, ScaledAttackRadius());
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

    #region Knock

    public void Knock()
    {
        StartCoroutine(Knockout(knockedForce, knockedDirection, knockedDuration));
    }
    
    public void Knock(Vector2 force, float direction, float duration)
    {
        StartCoroutine(Knockout(force, direction, duration));
    }

    public void SetupKnockDirection(Transform dmgSource)
    {
        knockedDirection = dmgSource.position.x > transform.position.x ? -1 : 1;
    }
    
    public IEnumerator Knockout(Vector2 force, float direction, float duration)
    {
        isKnocked = true;
        GetComponent<Rigidbody2D>().velocity = new Vector2(force.x * direction, force.y); // I have to use GetComponent here because I'm using a coroutine and I can't use the rb property because it's not initiated yet (coroutine starts before Start method)
        yield return new WaitForSeconds(duration);
        isKnocked = false;
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
    }

    #endregion
}
