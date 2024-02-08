using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity
{
    public PlayerInput input { get; private set; }
    public SkillManager skills { get; private set; }
    public GameObject sword { get; private set; }
    
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerStateIdle stateIdle { get; private set; }
    public PlayerStateMove stateMove { get; private set; }
    public PlayerStateJump stateJump { get; private set; }
    public PlayerStateAir stateAir { get; private set; }
    public PlayerStateDash stateDash { get; private set; }
    public PlayerStateWallSlide stateWallSlide { get; private set; }
    public PlayerStateWallJump stateWallJump { get; private set; }
    public PlayerStateAttack stateAttack { get; private set; }
    public PlayerStateCounterAttack stateCounterAttack { get; private set;}
    public PlayerStateAimSword stateAimSword  { get; private set;}
    public PlayerStateCatchSword stateCatchSword  { get; private set;}
    
    [Header("Move info")]
    [SerializeField] public float jumpForce;
    [SerializeField] public Vector2 wallJumpForce;

    [Header("CounterAttackInfo")]
    [SerializeField] public float counterAttackDuration;

    [Header("Attack info")]
    [SerializeField] public Vector2[] attackEnterForce;
    [SerializeField] public float comboTimer;
    
    public Vector2 moveVector {  get; private set; }
    
    public bool IsBusy {  get; private set; }
    
    protected override void Awake()
    {
        stateMachine = new PlayerStateMachine();

        stateIdle = new PlayerStateIdle(this, stateMachine, "Idle");
        stateMove = new PlayerStateMove(this, stateMachine, "Move");
        stateJump = new PlayerStateJump(this, stateMachine, "Air");
        stateAir = new PlayerStateAir(this, stateMachine, "Air");
        stateDash = new PlayerStateDash(this, stateMachine, "Dash");
        stateWallSlide = new PlayerStateWallSlide(this, stateMachine, "WallSlide");
        stateWallJump = new PlayerStateWallJump(this, stateMachine, "Air");
        stateAttack = new PlayerStateAttack(this, stateMachine, "Attack");
        stateCounterAttack = new PlayerStateCounterAttack(this, stateMachine, "CounterAttack");
        stateAimSword = new PlayerStateAimSword(this, stateMachine, "AimSword");
        stateCatchSword = new PlayerStateCatchSword(this, stateMachine, "CatchSword");
        
        moveVector = new Vector2(0, 0);
    }
    
    public void SetSword(GameObject _sword)
    {
        sword = _sword;
    }
    
    public void ClearSword()
    {
        Destroy(sword);
    }
    
    protected override void Start()
    {
        base.Start();
        input = GetComponent<PlayerInput>();
        
        skills = SkillManager.instance;
        
        stateMachine.Initialize(stateIdle);
    }
    
    protected override void Update()
    {
        base.Update();

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
        if (value.isPressed && skills.dash.CanUse())
        {
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
