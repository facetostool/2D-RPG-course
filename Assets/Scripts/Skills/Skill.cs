using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public Player player { get; private set; }
    
    [field: SerializeField] public float cooldown { get; private set; }
    private float cooldownTimer;
    
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
    }
    
}
