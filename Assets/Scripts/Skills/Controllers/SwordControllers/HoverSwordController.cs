using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverSwordController : BaseSwordController
{
    private bool isHovering;
    private bool isStopped;
    private float hoverGravityScale;
    private float hoverMaxDistance;
    private float hoverTime;
    private float hoverTimer;
    private float hoverHitTime;
    private float hoverHitTimer;
    
    public void Setup(
        Vector2 _throwForce,
        float _gravityScale, 
        float _returnSpeed, 
        float _hoverMaxDistance, 
        float _hoverTime, 
        float _hoverHitTime
    )
    {
        SetupBase(_throwForce, _gravityScale, _returnSpeed);
        
        hoverMaxDistance = _hoverMaxDistance;
        hoverTime = _hoverTime;
        hoverHitTime = _hoverHitTime;
        isStopped = false;
    }
    
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        
        if (isStopped)
        {
            hoverTimer -= Time.deltaTime;
            if (hoverTimer <= 0)
            {
                collider2D.enabled = false;
                isReturning = true;
                isStopped = false;
                return;
            }

            if (hoverHitTimer <= 0)
            {
                Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1);
                foreach (var hit in hits)
                {
                    Enemy enemy = hit.GetComponent<Enemy>();
                    if (enemy)
                    {
                        enemy.Damage();
                    }
                }
                hoverHitTimer = hoverHitTime;
                return;
            }
            hoverHitTimer -= Time.deltaTime;
            return;
        }
        
        if (Vector2.Distance(player.transform.position, transform.position) >= hoverMaxDistance)
        {
            StopSword();
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (!enabled) return;
        
        base.OnTriggerEnter2D(other);

        if (other.GetComponent<Enemy>() == null)
        {
            StuckSword(other);
            return;
        }
        StopSword();
    }

    private void StopSword()
    {
        hoverTimer = hoverTime;
        isStopped = true;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }
}
