using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EntityStats : MonoBehaviour
{
    private EntityFX fx;
    private Entity entity;
    
    [Header("Base Stats")]
    public Stat strength; // 1 point increases damage by 1 and crit power by 1%
    public Stat agility; // 1 point increases evasion by 1 and crit chance by 1%
    public Stat intelligence; // 1 point increases magic damage by 1 and magic resistance by 3
    public Stat vitality; // 1 point increases health by 5

    [Header("Defensive Stats")]
    public Stat armor;
    public Stat evasion;
    public Stat maxHealth;
    public Stat magicResistance;
    
    [Header("Offensive Stats")]
    public Stat damage;
    public Stat critChance;
    public Stat critPower; // 150% by default
    
    [Header("Magic Stats")]
    public Stat fireDamage;
    public float ignitedTime;
    public Stat iceDamage;
    public float chilledTime;
    
    public Stat lightningDamage;
    public float shockedTime;
    
    public bool isIgnited; // does damage over time
    public bool isChilled; // reduce armor by 20%
    public bool isShocked; // reduce evasion by 20%

    float ignitedTimer;
    float chilledTimer;
    float shockedTimer;
    
    float igniteDmgCooldown = 0.5f;
    float igniteDmgTimer;
    int igniteDmg;
    
    [SerializeField] private GameObject thunderStrikePrefab;
    int shockDmg;

    private bool isDead;
    public int currentHealth;
    public Action onHealthChanged;
    
    public virtual void Start()
    {
        currentHealth = maxHealth.Value();
        critPower.SetBaseValue(150);
        
        fx = GetComponentInChildren<EntityFX>();
        entity = GetComponent<Entity>();
    }

    public void Update()
    {
        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;
        
        igniteDmgTimer -= Time.deltaTime;
        
        if (ignitedTimer <= 0)
            isIgnited = false;
        
        if (chilledTimer <= 0)
            isChilled = false;
        
        if (shockedTimer <= 0)
            isShocked = false;

        if (igniteDmgTimer <= 0 && isIgnited)
            ApplyIgniteDamage();
    }

    private void ApplyIgniteDamage()
    {
        igniteDmgTimer = igniteDmgCooldown;
        UpdateHealth(currentHealth - igniteDmg);
        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }
    
    protected virtual void Die()
    {
        isDead = true;
    }
    
    public int MaxHealthValue()
    {
        return maxHealth.Value() + vitality.Value()*5;
    }

    private void UpdateHealth(int health)
    {
        currentHealth = health;
        onHealthChanged?.Invoke();
    }
    
    public virtual void DoDamage(EntityStats target)
    {
        if (IsAttackMissed(target))
            return;
        
        int finalDamage = damage.Value() + strength.Value() - target.ArmorValue();
        
        if (IsCritAttack())
            finalDamage = CalculateCritDamage(finalDamage);
        
        target.TakePhysicalDamage(finalDamage);
        // DoMagicDamage(target);
    }
    
    #region Magic Damage
    public void DoMagicDamage(EntityStats target)
    {
        int finalDmg = fireDamage.Value() + iceDamage.Value() + lightningDamage.Value();
        finalDmg -= target.magicResistance.Value() + target.intelligence.Value()*3;
        
        target.TakePhysicalDamage(finalDmg);
        
        if (fireDamage.Value() <= 0 && iceDamage.Value() <= 0 && lightningDamage.Value() <= 0)
            return;
        
        bool _isIgnited = fireDamage.Value() > iceDamage.Value() && fireDamage.Value() > lightningDamage.Value();
        bool _isChilled = iceDamage.Value() > fireDamage.Value() && iceDamage.Value() > lightningDamage.Value();
        bool _isShocked = lightningDamage.Value() > fireDamage.Value() && lightningDamage.Value() > iceDamage.Value();

        while (!_isIgnited && !_isChilled && !_isShocked)
        {   
            if (Random.value < 0.33f && fireDamage.Value() > 0)
                _isIgnited = true;
            else if (Random.value < 0.66f && iceDamage.Value() > 0)
                _isChilled = true;
            else
                _isShocked = true;
        }
        
        if (_isIgnited)
            target.AssignIgniteDmg(fireDamage.Value() * 0.2f);
        
        if (_isShocked)
            target.AssignShockDmg(lightningDamage.Value() * 0.2f);
        
        target.ApplyAilments(_isIgnited, _isChilled, _isShocked);
    }
    
    public void AssignIgniteDmg(float _igniteDmg)
    {
        igniteDmg = Mathf.RoundToInt(_igniteDmg);
    }
    
    public void AssignShockDmg(float _shockDmg)
    {
        shockDmg = Mathf.RoundToInt(_shockDmg);
    }
    
    public void ApplyAilments(bool _isIgnited, bool _isChilled, bool _isShocked)
    {
        bool canApplyIgnite = !isIgnited && !isChilled && !isShocked;
        bool canApplyChill = !isIgnited && !isChilled && !isShocked;
        bool canApplyShock = !isIgnited && !isChilled;
        
        if (_isIgnited && canApplyIgnite)
        {
            isIgnited = true;
            ignitedTimer = ignitedTime;
            fx.IgniteEffectFor(ignitedTime);
            
        }
        
        if (_isChilled && canApplyChill)
        {
            isChilled = true;
            chilledTimer = chilledTime;
            fx.ChillEffectFor(chilledTime);
            entity.SlowBy(0.5f, chilledTime);
        }
        
        if (_isShocked && canApplyShock)
        {
            if (!isShocked)
            {
                isShocked = true;
                shockedTimer = shockedTime;
                fx.ShockEffectFor(shockedTime);
                return;
            }

            if (GetComponent<Player>() != null)
            {
                return;
            }
            
            Enemy closesEnemy = FindClosestEnemyForThunderStrike();
            if (closesEnemy)
            {
                ThunderStrikeController tsc = Instantiate(thunderStrikePrefab, transform.position, Quaternion.identity).GetComponent<ThunderStrikeController>();
                tsc.Setup(closesEnemy.gameObject, 15, shockDmg);
            }
        }
    }
    
    Enemy FindClosestEnemyForThunderStrike()
    {
        float shortestDistance = 10;
        Enemy closestDifferentEnemy = null;
        Vector3 entityPosition = entity.transform.position;
        Collider2D[] hits = Physics2D.OverlapCircleAll(entityPosition, 10);
        foreach (var hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy)
            {
                float distance = Math.Abs(entityPosition.x - enemy.transform.position.x);
                if (distance < shortestDistance && distance > 0.2f)
                {
                    shortestDistance = distance;
                    closestDifferentEnemy = enemy;
                }
            }
        }

        return closestDifferentEnemy;
    }
    
    #endregion Magic Damage

    #region  Physical Damage Calculations
    
    public virtual void TakePhysicalDamage(int dmg)
    {
        if (dmg <= 0)
            return;
        
        GetComponent<Entity>().DamageEffect();
        
        UpdateHealth(currentHealth - dmg);
        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }
    
    private bool IsAttackMissed(EntityStats target)
    {
        int totalTargetEvasion = target.evasion.Value() + target.agility.Value();
        
        if (isShocked)
            totalTargetEvasion += 20;
        
        int hitChance = Random.Range(0, 100);
        if (hitChance < totalTargetEvasion)
        {
            Debug.Log("Character evaded the attack!");
            return true;
        }

        return false;
    }

    private bool IsCritAttack()
    {
        int finalCritChanceValue = critChance.Value() + agility.Value();
        if (Random.Range(0, 100) < finalCritChanceValue)
        {
            Debug.Log("Character landed a critical hit!");
            return true;
        }

        return false;
    }
    
    private int CalculateCritDamage(int damage)
    {
        float critDmg = damage * (critPower.Value() + strength.Value()) / 100;
        return Mathf.RoundToInt(critDmg);
    }

    public int ArmorValue()
    {
        return isChilled ? Mathf.RoundToInt(armor.Value()*0.8f) : armor.Value();
    }
    
    #endregion
}
