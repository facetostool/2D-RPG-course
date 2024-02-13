using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : Entity
{
    public EnemyStateMachine stateMachine;
    
    [Header("Knocked info")]
    [SerializeField] private Vector2 knockedDirection;
    [SerializeField] private float knockedDuration;
    private bool isKnocked;
    
    [Header("Stunned info")]
    [SerializeField] public Vector2 stunnedDirection;
    [SerializeField] public float stunnedDuration;
    [SerializeField] public SpriteRenderer counterAttackIndicator;
    
    public bool canBeStunned;
    
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        
        stateMachine.currentState.Update();
    }
    
    public IEnumerator Knockout()
    {
        isKnocked = true;
        rb.velocity = new Vector2(knockedDirection.x * -faceDir, knockedDirection.y);
        yield return new WaitForSeconds(knockedDuration);
        isKnocked = false;
        base.SetVelocity(0, 0);
    }

    public void OnHitAttackTrigger()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(this.attackCheck.position, this.attackCheckRadius);
        foreach (var hit in hits)
        {
            Player player = hit.GetComponent<Player>();
            if (player)
            {
                player.Damage();
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

    public override void Damage()
    {
        base.Damage();
        StartCoroutine("Knockout");
    }
}