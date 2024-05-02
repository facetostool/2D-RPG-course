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
    // private bool finishedAnimation;

    [SerializeField] private Transform attackCheck;
    
    public void Setup(Vector3 _position, float _disappearSpeed, Transform enemyTransform)
    {
        transform.position = _position;
        disappearSpeed = _disappearSpeed;
        
        if (enemyTransform && enemyTransform.position.x < transform.position.x)
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
    
    public void CancelAnimationTrigger()
    {
        // finishedAnimation = true;
        
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
                // enemy.StartCoroutine("Knockout");
                player.stats.DoDamage(enemy.stats);
            }
        }
    }
}