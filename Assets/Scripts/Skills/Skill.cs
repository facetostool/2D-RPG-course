using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public Player player { get; private set; }
    
    [SerializeField] public SkillImageCooldown uiImageCooldown;
    
    [field: SerializeField] public float cooldown { get; set; }
    public float cooldownTimer;
    
    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
    }
    
    protected virtual void Update()
    {
        if (cooldownTimer >= 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    public virtual bool CanUse()
    {
        return cooldownTimer <= 0;
    }

    public virtual void Use()
    {
        cooldownTimer = cooldown;

        StartImageCooldown(cooldown);
    }
    
    protected void StartImageCooldown(float _cooldown)
    {
        if (uiImageCooldown)
            uiImageCooldown.StartCooldown(_cooldown);
    }
    
    public Transform ClosestEnemyPosition(Vector3 entityPosition)
    {
        float shortestDistance = 10;
        Transform closestTransform = null;
        Collider2D[] hits = Physics2D.OverlapCircleAll(entityPosition, 10);
        foreach (var hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy && !enemy.IsDead())
            {
                float distance = Math.Abs(entityPosition.x - enemy.transform.position.x);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    closestTransform = hit.transform;
                }
            }
        }

        return closestTransform;
    }
}
