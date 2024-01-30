using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Animator animator { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public EntityFlashFX fx {get; private set;}
    
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
    
    
    public virtual void Awake()
    {
        
    }

    public virtual void Start()
    {
        fx = GetComponentInChildren<EntityFlashFX>();
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    
    protected virtual void Update()
    {
        
    }
    
    public void Flip()
    {
        faceDir = -faceDir;
        transform.Rotate(0, 180, 0);
    }
    
    public void SetVelocity(float x, float y)
    {
        rb.velocity = new Vector2(x, y);
    }

    public virtual void Damage()
    {
        fx.StartCoroutine("Flash");
        
        Debug.Log(gameObject.name + " damaged");
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
