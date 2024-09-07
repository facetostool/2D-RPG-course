using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : Entity
{
    public EnemyStateMachine stateMachine;
    
    [Header("Stunned info")]
    [SerializeField] public Vector2 stunnedDirection;
    [SerializeField] public float stunnedDuration;
    [SerializeField] public SpriteRenderer counterAttackIndicator;
    [SerializeField] public float stunnedKnockDuration;
    
    [Header("Attack info")]
    [SerializeField] public float attackMinCooldown;
    [SerializeField] public float attackMaxCooldown;
    
    public bool canBeStunned;
    
    private float defaultMoveSpeed;
    private float defaultAnimationSpeed;
    
    protected override void Start()
    {
        base.Start();
        
        defaultMoveSpeed = moveSpeed;
        defaultAnimationSpeed = animator.speed;
    }

    protected override void Update()
    {
        base.Update();
        
        stateMachine.currentState.Update();
    }

    public void OnHitAttackTrigger()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(this.attackCheck.position, this.attackCheckRadius);
        foreach (var hit in hits)
        {
            Player player = hit.GetComponent<Player>();
            if (player)
            {
                stats.DoDamage(player.stats, transform);
            }
        }
    }

    public override void SetVelocity(float x, float y)
    {
        if (!isKnocked)
        {
            base.SetVelocity(x, y);
        }
    }

    public void OpenCounterAttackWindow()
    {
        canBeStunned = true;
        counterAttackIndicator.enabled = true;
    }
    
    public void CloseCounterAttackWindow()
    {
        canBeStunned = false;
        counterAttackIndicator.enabled = false;
    }

    public virtual void Stun()
    {
        CloseCounterAttackWindow();
    }

    public override void DamageEffect()
    {
        base.DamageEffect();
        StartCoroutine(nameof(Knockout));
    }
    
    public void Freeze()
    {
        moveSpeed = 0;
        animator.speed = 0;
    }
    
    public void Unfreeze()
    {
        moveSpeed = defaultMoveSpeed;
        animator.speed = defaultAnimationSpeed;
    }
    
    public void FreezeFor(float seconds)
    {
        StartCoroutine(nameof(FreezeForCoroutine), seconds);
    }
    
    public IEnumerator FreezeForCoroutine(float seconds)
    {
        Freeze();
        yield return new WaitForSeconds(seconds);
        Unfreeze();
    }

    public virtual void Die()
    {
    }
    
    public void OnDieAnimationEnd()
    {
        DestroyObject();
    }
    
    public void DestroyObject()
    {
        Destroy(gameObject);
    }

    public override void SlowBy(float slowAmount, float slowTime)
    {
        base.SlowBy(slowAmount, slowTime);
        moveSpeed = moveSpeed * (1 - slowAmount);
        animator.speed = animator.speed * (1 - slowAmount);
        
        Invoke(nameof(ReturnDefaultSpeed), slowTime);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();
        moveSpeed = defaultMoveSpeed;
        animator.speed = defaultAnimationSpeed;
    }
}