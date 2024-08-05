using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillClone : Skill
{
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float disappearSpeed;
    [SerializeField] private float attackDmgMultiplier;
    
    [Header("Clone Attack")]
    [SerializeField] private SkillTreeSlot cloneAttackSlot;
    [SerializeField] private bool cloneAttackUnlocked;
    [SerializeField] private float cloneAttackDmgMultiplier;
    
    [Header("Aggressive Clone")]
    [SerializeField] private SkillTreeSlot aggressiveCloneSlot;
    [SerializeField] private bool aggressiveCloneUnlocked;
    [SerializeField] private float aggressiveCloneDmgMultiplier;
    
    [Header("Multiple Clones")]
    [SerializeField] private SkillTreeSlot multipleClonesSlot;
    [SerializeField] private bool multipleClonesUnlocked;
    [SerializeField] private float multipleClonesDmgMultiplier;
    [SerializeField] private float cloneSpawnChance;
    
    [Header("Crystal Clone")]
    [SerializeField] private SkillTreeSlot crystalCloneSlot;
    [SerializeField] private bool crystalCloneUnlocked;
    
    protected override void Start()
    {
        base.Start();
        
        cloneAttackSlot.onUnlock += () =>
        {
            cloneAttackUnlocked = true;
            attackDmgMultiplier = cloneAttackDmgMultiplier;
        };
        aggressiveCloneSlot.onUnlock += () =>
        {
            aggressiveCloneUnlocked = true;
            attackDmgMultiplier = aggressiveCloneDmgMultiplier;
        };
        multipleClonesSlot.onUnlock += () =>
        {
            multipleClonesUnlocked = true;
            attackDmgMultiplier = multipleClonesDmgMultiplier;
        };
        crystalCloneSlot.onUnlock += () =>
        {
            crystalCloneUnlocked = true;
        };
    }
    
    public void Use(Vector3 position, Transform enemyPosition)
    {
        base.Use();

        if (crystalCloneUnlocked)
        {
            player.skills.crystal.CreateCrystal(player.transform.position);
            return;
        }
        GameObject cloneObject = Instantiate(clonePrefab);
        cloneObject.GetComponent<CloneController>().Setup(position, disappearSpeed, enemyPosition, cloneSpawnChance, cloneAttackUnlocked);
    }
    
    public void UseWithDelayAndOffset(Transform _hit)
    {
        StartCoroutine(UseWithDelayCoroutine(new Vector3(_hit.position.x + player.faceDir * 1.5f, _hit.position.y), _hit, 0.4f));
    }

    public void UseWithRandomOffsetAndDelay(Transform _hit, float delay = 0.1f)
    {
        int faceDir = -1;
        if (Random.Range(0, 100) < 50)
        {
            faceDir = 1;
        }

        StartCoroutine(UseWithDelayCoroutine(new Vector3(_hit.position.x + faceDir * 1.5f, _hit.position.y), _hit, delay));
    }
    
    public IEnumerator UseWithDelayCoroutine(Vector3 position, Transform enemyPosition, float delay)
    {
        yield return new WaitForSeconds(delay);
        Use(position, enemyPosition);
    }
    
    public void OnHitAttackTrigger(Enemy enemy)
    {
        enemy.StartCoroutine("Knockout");
        PlayerStats playerStats = (PlayerStats)player.stats;

        playerStats.DoCloneDamage((EnemyStats)enemy.stats, attackDmgMultiplier);
            
        if (multipleClonesUnlocked)
        {
            if (Random.Range(0f, 1f) <= cloneSpawnChance)
            {
                UseWithRandomOffsetAndDelay(enemy.transform, 0.2f);
            }
        }
        
        if (aggressiveCloneUnlocked)
        {
            if (InventoryManager.instance.equipmentItemDict.TryGetValue(EquipmentType.Weapon, out var weapon))
            {
                if (weapon != null)
                {
                    var itemData = weapon.itemData as ItemDataEquipment;
                    itemData?.ApplyEffects(enemy.transform);
                }
            }
        }
    }
}
