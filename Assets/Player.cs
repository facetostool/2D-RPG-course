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
    public PlayerStateDash stateDash { get; private set; }
    public PlayerStateWallSlide stateWallSlide { get; private set; }
    public PlayerStateWallJump stateWallJump { get; private set; }
    public PlayerStateAttack StateAttack { get; private set; }

    [Header("Move info")]
    [SerializeField] public float moveSpeed;
    [SerializeField] public float jumpForce;
    [SerializeField] public Vector2 wallJumpForce;
    public float faceDir = 1;

    [Header("Dash info")]
    [SerializeField] public float dashSpeed;
    [SerializeField] public float dashTime;
    [SerializeField] public float dashCooldown;
    private float lastDashTime = 0;

    [Header("Collisions")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance;
    
    public Vector2 moveVector {  get; private set; }
    
    void Awake()
    {
        stateMachine = new PlayerStateMachine();

        stateIdle = new PlayerStateIdle(this, stateMachine, "Idle");
        stateMove = new PlayerStateMove(this, stateMachine, "Move");
        stateJump = new PlayerStateJump(this, stateMachine, "Air");
        stateAir = new PlayerStateAir(this, stateMachine, "Air");
        stateDash = new PlayerStateDash(this, stateMachine, "Dash");
        stateWallSlide = new PlayerStateWallSlide(this, stateMachine, "WallSlide");
        stateWallJump = new PlayerStateWallJump(this, stateMachine, "Air");
        StateAttack = new PlayerStateAttack(this, stateMachine, "Attack");
        
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

    void OnDash(InputValue value)
    {
        if (value.isPressed && ((Time.time - lastDashTime) > dashCooldown || Time.time < dashCooldown))
        {
            lastDashTime = Time.time;
            stateMachine.ChangeState(stateDash);
        }
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

   public void Flip()
   {
       faceDir = -faceDir;
       transform.Rotate(0, 180, 0);
   }

   private void OnDrawGizmos()
   {
       Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
       Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance*faceDir, wallCheck.position.y));
   }

   public bool IsGroundDetected()
   {
       return Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
   }

   public bool IsWallDetected()
   {
       return Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance*faceDir, whatIsGround);
   }
   
   public void SetVelocity(float x, float y)
   {
       rb.velocity = new Vector2(x, y);
   }
}
