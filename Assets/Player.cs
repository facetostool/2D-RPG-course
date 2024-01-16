using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Animator animator { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public PlayerInput input { get; private set; }
    
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerStateIdle stateIdle { get; private set; }
    public PlayerStateMove stateMove { get; private set; }
    public PlayerStateJump stateJump { get; private set; }
    public PlayerStateAir stateAir { get; private set; }

    [Header("Move info")]
    [SerializeField] public float moveSpeed;
    [SerializeField] public float jumpForce;
    public float faceDir = 1;

    [Header("Collisions")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    
    public Vector2 moveVector {  get; private set; }
    
    void Awake()
    {
        stateMachine = new PlayerStateMachine();

        stateIdle = new PlayerStateIdle(this, stateMachine, "Idle");
        stateMove = new PlayerStateMove(this, stateMachine, "Move");
        stateJump = new PlayerStateJump(this, stateMachine, "Air");
        stateAir = new PlayerStateAir(this, stateMachine, "Air");
        
        moveVector = new Vector2(0, 0);
    }
    
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInput>();
        
        stateMachine.Initialize(stateIdle);
    }
    
    void Update()
    {
        stateMachine.currentState.Update();

        FlipController();
    }

    void OnMove(InputValue value)
    {
        moveVector = value.Get<Vector2>();
    }

    void FlipController()
    {
        if (rb.velocity.x > 0 && faceDir < 0)
        {
            Flip();
        }

        if (rb.velocity.x < 0 && faceDir > 0)
        {
            Flip();
        }
    }

   void Flip()
   {
       faceDir = -faceDir;
       transform.Rotate(0, 180, 0);
   }

   private void OnDrawGizmos()
   {
       Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
   }

   public bool IsGroundDetected()
   {
       return Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
   }
}
