using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneController : MonoBehaviour
{
    private float disappearSpeed;

    private SpriteRenderer sr;
    private Animator animator;
    private Player player;
    
    private bool canAttack;
    private float cloneSpawnChance;
    // private bool finishedAnimation;

    [SerializeField] private Transform attackCheck;
    
    public void Setup(Vector3 _position, float _disappearSpeed, Transform enemyTransform, float cloneSpawnChance, bool canAttack = false)
    {
        transform.position = _position;
        disappearSpeed = _disappearSpeed;
        this.canAttack = canAttack;
        this.cloneSpawnChance = cloneSpawnChance;
        
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

        if (canAttack)
        {
            animator.SetBool("Attack", canAttack);
            animator.SetInteger("AttackCounter", UnityEngine.Random.Range(0, 3));
            SoundManager.instance.PlaySFX( SfxEffect.PlayerAttack3);
        }
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
    
    public void FinishAnimationTrigger()
    {
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
               player.skills.clone.OnHitAttackTrigger(enemy);
            }
        }
    }
}