using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceSwordController : BaseSwordController
{
    private int bounceNumber;
    private float bounceRadius;
    private float bounceSpeed;
    private int currentBounceTargetNumber;
    private List<Transform> bouncesEnemies = new List<Transform>();
    
    public void Setup(
        Vector2 _throwForce,
        float _gravityScale,
        float _returnSpeed,
        float _freezeTime,
        int _bounceNumber,
        float _bounceRadius,
        float _bounceSpeed
    )
    {
        SetupBase(_throwForce, _gravityScale, _returnSpeed, _freezeTime);
        
        bounceNumber = _bounceNumber;
        bounceRadius = _bounceRadius;
        bounceSpeed = _bounceSpeed;
    }
    
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if (bouncesEnemies.Count == 1 && !isReturning)
        {
            RegisterDamage(bouncesEnemies[currentBounceTargetNumber].GetComponent<Enemy>());
            isReturning = true;
            bounceNumber = 0;
        }
        
        if (bouncesEnemies.Count > 0 && bounceNumber > 0)
        {
            transform.position = Vector2.MoveTowards(
                transform.position, 
                bouncesEnemies[currentBounceTargetNumber].position,
                bounceSpeed * Time.deltaTime
            );
                
            if (Vector2.Distance(transform.position, bouncesEnemies[currentBounceTargetNumber].position) < 0.1f)
            {
                currentBounceTargetNumber++;
                bounceNumber--;
                RegisterDamage(bouncesEnemies[currentBounceTargetNumber].GetComponent<Enemy>());
                if (currentBounceTargetNumber >= (bouncesEnemies.Count - 1))
                {
                    currentBounceTargetNumber = 0;
                }
                if (bounceNumber <= 0)
                {
                    isReturning = true;
                }
            }
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (!enabled) return;
        
        base.OnTriggerEnter2D(other);
        
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy == null)
        {
            StuckSword(other);
            return;
        }

        RegisterDamage(enemy);
        
        bouncesEnemies = new List<Transform>();
        Collider2D[] hits = Physics2D.OverlapCircleAll(enemy.transform.position, bounceRadius);
        foreach (var hit in hits)
        {
            Enemy enemyInRadius = hit.GetComponent<Enemy>();
            if (enemyInRadius)
            {
                bouncesEnemies.Add(enemyInRadius.transform);
            }
        }
        
        collider2D.enabled = false;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }
}
