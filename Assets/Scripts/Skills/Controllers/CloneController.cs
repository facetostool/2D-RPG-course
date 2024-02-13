using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class CloneController : MonoBehaviour
{
    private float disappearSpeed;

    private SpriteRenderer sr;
    private Animator animator;
    private Player player;
    private bool finishedAnimation;

    [SerializeField] private Transform attackCheck;
    
    public void Setup(float _disappearSpeed)
    {
        disappearSpeed = _disappearSpeed;

        Vector3 closesEnemyPosition = ClosestEnemyPosition();
        if (closesEnemyPosition.x < transform.position.x)
        {
            transform.Rotate(0, 180, 0);
        }
    }
    
    void Start()
    {
        player = PlayerManager.instance.player;
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        
        animator.SetBool("Attack", true);
        animator.SetInteger("AttackCounter", UnityEngine.Random.Range(0, 3));
    }
    
    void Update()
    {
        if (finishedAnimation)
        {
            if (sr.color.a <= 0)
            {
                Destroy(gameObject);
                return;
            }
        
            if (sr.color.a >= 0)
            {
                Color tmp = sr.color;
                tmp.a = sr.color.a - Time.deltaTime * disappearSpeed;
                sr.color = tmp;
            }
        }
    }
    
    public void CancelAnimationTrigger()
    {
        finishedAnimation = true;
        
        animator.SetBool("Attack", false);
    }

    public void OnHitAttackTrigger()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackCheck.position, player.attackCheckRadius);
        foreach (var hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy)
            {
                enemy.StartCoroutine("Knockout");
                enemy.Damage();
            }
        }
    }

    private Vector3 ClosestEnemyPosition()
    {
        float shortestDistance = 10;
        Vector3 closestPosition = transform.position;
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 10);
        foreach (var hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy)
            {
                float distance = Math.Abs(transform.position.x - enemy.transform.position.x);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    closestPosition = hit.transform.position;
                }
            }
        }

        return closestPosition;
    }
}
