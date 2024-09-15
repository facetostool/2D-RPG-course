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
    
    public InventoryManager inventory { get; private set; }
    
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
    public PlayerStateUltimate stateUltimate { get; private set; }
    public PlayerStateDead stateDead { get; private set; }
    
    public PlayerStateKnocked stateKnocked { get; private set; }
    
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
    
    float defaultMoveSpeed;
    float defaultJumpForce;
    float defaultDashSpeed;
    Vector2 defaultWallJumpForce;
    
    public Action onDie;
    
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
        stateUltimate = new PlayerStateUltimate(this, stateMachine, "Ultimate");
        stateDead = new PlayerStateDead(this, stateMachine, nameof(Die));
        stateKnocked = new PlayerStateKnocked(this, stateMachine, "Air");
        
        moveVector = new Vector2(0, 0);
    }
    
    public void SetSword(GameObject _sword)
    {
        sword = _sword;
    }
    
    public void CatchSword()
    {
        stateMachine.ChangeState(stateCatchSword);
        Destroy(sword);
    }
    
    protected override void Start()
    {
        base.Start();
        input = GetComponent<PlayerInput>();
        
        skills = SkillManager.instance;
        inventory = InventoryManager.instance;
        
        stateMachine.Initialize(stateIdle);
        
        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
        defaultDashSpeed = skills.dash.dashSpeed;
        defaultWallJumpForce = wallJumpForce;
    }
    
    protected override void Update()
    {
        base.Update();
        
        if (GameManager.instance.IsGamePaused())
            return;

        stateMachine.currentState.Update();
        FlipController();

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (inventory.CanUseFlask())
            {
                inventory.UseFlask();
            }
        }
    }

    public IEnumerator BusyFor(float _time)
    {
        IsBusy = true;
        yield return new WaitForSeconds(_time);
        IsBusy = false;
    }
    
    public void MakeBusyFor(float _time)
    {
        StartCoroutine(BusyFor(_time));
    }

    void OnDash(InputValue value)
    {
        if (IsBusy)
            return;
        
        if (GameManager.instance.IsGamePaused())
            return;
        
        if (value.isPressed && skills.dash.CanUse())
        {
            stateMachine.ChangeState(stateDash);
        }
    }

    void OnCrystal(InputValue value)
    {
        if (IsBusy)
            return;
        
        if (GameManager.instance.IsGamePaused())
            return;
        
        if (value.isPressed && skills.crystal.CanUse())
        {
            skills.crystal.Use();
        }
    }

    void OnMove(InputValue value)
    {
        if (GameManager.instance.IsGamePaused())
            return;
        
        moveVector = value.Get<Vector2>();
    }

    void FlipController()
    {
        if (IsBusy)
            return;
        
        if (rb.velocity.x > 0 && faceDir < 0)
        {
            Flip();
        }

        if (rb.velocity.x < 0 && faceDir > 0)
        {
            Flip();
        }
    }
    
    public void ExitUltimate()
    {   
        MakeVisible();
        stateMachine.ChangeState(stateIdle);
    }

    public void Die()
    {
        stateMachine.ChangeState(stateDead);
        onDie?.Invoke();
    }

    public override void SlowBy(float slowAmount, float slowTime)
    {
        base.SlowBy(slowAmount, slowTime);
        moveSpeed = moveSpeed * (1 - slowAmount);
        jumpForce = jumpForce * (1 - slowAmount);
        skills.dash.dashSpeed = skills.dash.dashSpeed * (1 - slowAmount);
        wallJumpForce = wallJumpForce * (1 - slowAmount);
        animator.speed = animator.speed * (1 - slowAmount);
        
        Invoke(nameof(ReturnDefaultSpeed), slowTime);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();
        
        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        skills.dash.dashSpeed = defaultDashSpeed;
        wallJumpForce = defaultWallJumpForce;
        animator.speed = 1;
    }
    
    public int GetAttackCounter()
    {
        return stateAttack.attackCounter;
    }
    
    public override void DamageEffect(int dmg)
    {
        base.DamageEffect(dmg);
        if (dmg * 100 /stats.MaxHealthValue() >= 30)
        {
            stateMachine.ChangeState(stateKnocked);
        }
        else
        {
            SoundManager.instance.PlaySFX(SfxEffect.WomenSigh2);
        }
    }
}
