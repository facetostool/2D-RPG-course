using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum StatType
{
    strength,
    agility,
    intelligence,
    vitality,
    armor,
    evasion,
    magicResistance,
    damage,
    critChance,
    critPower,
    fireDamage,
    iceDamage,
    lightningDamage,
    maxHealth,
}

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

    [SerializeField] private float vulnerableMultiplier = 1.2f;
    private bool isVulnerable;
    private bool isDead;
    public int currentHealth;
    public Action onHealthChanged;
    public Action onStatsChanged;
    
    public virtual void Start()
    {
        currentHealth = maxHealth.Value();
        onHealthChanged?.Invoke();
        
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
        fx.PopupTextFX(igniteDmg.ToString(), fx.fireDmgTextColor, fx.defaultPopupTextSize);
        TakeDamage(igniteDmg);
    }
    
    public bool IsDead()
    {
        return isDead;
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
    
    public virtual void TakeDamage(int dmg)
    {
        if (isVulnerable)
            dmg = Mathf.RoundToInt(vulnerableMultiplier * dmg);
        
        UpdateHealth(currentHealth - dmg);
        if (currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }

    public void HealByMax(float percentage)
    {
        var newHealth = currentHealth + Mathf.RoundToInt(MaxHealthValue() * percentage);
        if (newHealth > MaxHealthValue())
            newHealth = MaxHealthValue();
        
        UpdateHealth(newHealth);
    }
    
    public void BuffStatFor(float duration, Stat stat, int value)
    {
        stat.AddModifier(value);
        onStatsChanged?.Invoke();
        StartCoroutine(RemoveBuffAfter(duration, stat, value));
    }
    
    IEnumerator RemoveBuffAfter(float duration, Stat stat, int value)
    {
        yield return new WaitForSeconds(duration);
        stat.RemoveModifier(value);
        onStatsChanged?.Invoke();
    }

    public virtual void EvasionEffect(EntityStats attacker, EntityStats target)
    {
        fx.PopupTextFX("Evaded", fx.defaultPopupTextColor, fx.defaultPopupTextSize);
        Debug.Log("EvasionEffect");
    }
    
    public void MakeVulnerableFor(float duration)
    {
        StartCoroutine(VulnerableCoroutine(duration));
    }

    private IEnumerator VulnerableCoroutine(float duration)
    {
        isVulnerable = true;
        yield return new WaitForSeconds(duration);
        isVulnerable = true;
    }
    
    public virtual void DoDamage(EntityStats target, Transform dmgSource)
    {
        target.entity.SetupKnockDirection(dmgSource);
        
        if (IsAttackMissed(target))
        {
            target.EvasionEffect(this, target);
            return;
        }
        
        
        int finalDamage = GetDamage() - target.ArmorValue();

        var isCritical = IsCritAttack();
        if (isCritical)
            finalDamage = CalculateCritDamage(finalDamage);

        target.TakePhysicalDamage(finalDamage, isCritical);
        // DoMagicDamage(target);
    }
    
    public int GetDamage()
    {
        return damage.Value() + strength.Value();
    }
    
    public int GetMagicResistance()
    {
        return magicResistance.Value() + intelligence.Value()*3;
    }
    
    #region Magic Damage
    public void DoMagicDamage(EntityStats target, Transform dmgSource)
    {
        target.entity.SetupKnockDirection(dmgSource);
        
        int finalDmg = fireDamage.Value() + iceDamage.Value() + lightningDamage.Value();
        finalDmg -= target.GetMagicResistance();
        
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
    
    public virtual void TakePhysicalDamage(int dmg, bool isCritical = false)
    {
        if (dmg <= 0)
            return;
        
        GetComponent<Entity>().DamageEffect(dmg, isCritical);
        TakeDamage(dmg);
    }
    
    protected bool IsAttackMissed(EntityStats target)
    {
        int totalTargetEvasion = target.GetEvadeChance();
        
        if (isShocked)
            totalTargetEvasion += 20;
        
        int hitChance = Random.Range(0, 100);
        if (hitChance >= totalTargetEvasion) return false;
        
        entity.fx.PopupTextFX("Evaded", Color.white, entity.fx.defaultPopupTextSize);
        return true;
    }
    
    public int GetEvadeChance()
    {
        return evasion.Value() + agility.Value();
    }

    protected bool IsCritAttack()
    {
        if (Random.Range(0, 100) >= GetCritChance()) return false;
        Debug.Log("Character landed a critical hit!");
        return true;

    }
    
    public int GetCritChance()
    {
        return critChance.Value() + agility.Value();
    }
    
    protected int CalculateCritDamage(int damage)
    {
        float critDmg = GetCritPower() * damage / 100;
        return Mathf.RoundToInt(critDmg);
    }
    
    public int GetCritPower()
    {
        return critPower.Value() + strength.Value();
    }

    public int ArmorValue()
    {
        return isChilled ? Mathf.RoundToInt(armor.Value()*0.8f) : armor.Value();
    }
    
    #endregion
    
    public Stat GetStat(StatType type)
    {
        return type switch
        {
            StatType.strength => strength,
            StatType.agility => agility,
            StatType.intelligence => intelligence,
            StatType.vitality => vitality,
            StatType.armor => armor,
            StatType.evasion => evasion,
            StatType.magicResistance => magicResistance,
            StatType.damage => damage,
            StatType.critChance => critChance,
            StatType.critPower => critPower,
            StatType.fireDamage => fireDamage,
            StatType.iceDamage => iceDamage,
            StatType.lightningDamage => lightningDamage,
            StatType.maxHealth => maxHealth,
            _ => null
        };
    }
    
    public void AddModifier(StatType type, int value)
    {
        GetStat(type).AddModifier(value);
        onStatsChanged?.Invoke();
    }
}
