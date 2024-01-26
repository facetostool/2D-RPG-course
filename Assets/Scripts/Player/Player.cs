using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity
{
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
   
    [SerializeField] public float jumpForce;
    [SerializeField] public Vector2 wallJumpForce;


    [Header("Dash info")]
    [SerializeField] public float dashSpeed;
    [SerializeField] public float dashTime;
    [SerializeField] public float dashCooldown;
    private float lastDashTime = 0;
    

    [Header("Attack info")]
    [SerializeField] public Vector2[] attackEnterForce;
    [SerializeField] public float comboTimer;
    
    public Vector2 moveVector {  get; private set; }
    
    public bool IsBusy {  get; private set; }
    
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
    
    public override void Start()
    {
        base.Start();
        input = GetComponent<PlayerInput>();
        
        stateMachine.Initialize(stateIdle);
    }
    
    void Update()
    {
        stateMachine.currentState.Update();

        FlipController();
    }

    public IEnumerator BusyFor(float _time)
    {
        IsBusy = true;
        yield return new WaitForSeconds(_time);
        IsBusy = false;
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
}
