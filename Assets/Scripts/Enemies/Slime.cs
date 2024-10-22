using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum SlimeType
{
    Normal,
    Big,
    Small
}

public class Slime : Enemy
{
    [Header("Slime info")]
    [SerializeField] private SlimeType slimeType;
    [SerializeField] private int slimesToSpawn;
    [SerializeField] private GameObject smallerSlimePrefab;
    [SerializeField] private Vector2 minMaxXThrowForce;
    [SerializeField] private Vector2 minMaxYThrowForce;
    
    #region Movement
    [Header("Slime info")]
    [SerializeField] public float idleStateTime;
    [SerializeField] public float battleNoDetectionTime;
    #endregion
    
    #region States

    public SlimeStateIdle stateIdle { get; private set; }
    public SlimeStateMove stateMove { get; private set; }
    public SlimeStateBattle stateBattle { get; private set; }
    public SlimeStateAttack stateAttack { get; private set; }
    public SlimeStateStunned stateStunned { get; private set; }
    public SlimeStateDead stateDead { get; private set; }
    
    #endregion

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new EnemyStateMachine();
        stateIdle = new SlimeStateIdle(this, stateMachine, "Idle", this);
        stateMove = new SlimeStateMove(this, stateMachine, "Move", this);
        stateBattle = new SlimeStateBattle(this, stateMachine, "Move", this);
        stateAttack = new SlimeStateAttack(this, stateMachine, "Attack", this);
        stateStunned = new SlimeStateStunned(this, stateMachine, "Stunned", this);
        stateDead = new SlimeStateDead(this, stateMachine, "Die", this);
    }

    protected override void Start()
    {
        base.Start();
        
        stateMachine.Initialize(stateIdle);
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.H))
        {
            SpawnSmallerSlimes();
        }
    }

    public override void Stun()
    {
        base.Stun();
        
        stateMachine.ChangeState(stateStunned);
    }
    
    public override void Die()
    {
        base.Die();
        
        stateMachine.ChangeState(stateDead);
    }
    
    public void SpawnSmallerSlimes()
    {
        switch(slimeType)
        {
            case SlimeType.Big:
                for (int i = 0; i < slimesToSpawn; i++)
                {
                    SpawnSlime();
                }
                break;
            case SlimeType.Normal:
                for (int i = 0; i < slimesToSpawn; i++)
                {
                    SpawnSlime();
                }
                break;
            case SlimeType.Small:
                break;
        }
    }
    
    private void SpawnSlime()
    {
        SoundManager.instance.PlaySFX(SfxEffect.SlimeSpawn);
        
        GameObject slimeObj = Instantiate(smallerSlimePrefab, transform.position, Quaternion.identity);
        var slime = slimeObj.GetComponent<Slime>();
        
        var force = new Vector2(Random.Range(minMaxXThrowForce.x, minMaxXThrowForce.y), Random.Range(minMaxYThrowForce.x, minMaxYThrowForce.y));
        var dir = Random.Range(0, 100) < 50 ? -1 : 1;
        
        slime.Knock(force, dir, 0.5f);
        
        Invoke(nameof(Battle), 0.4f);
    }

    public override void OnDieAnimationEnd()
    {
        base.OnDieAnimationEnd();
        SpawnSmallerSlimes();
    }
    
    private void Battle()
    {
        stateMachine.ChangeState(stateAttack);
    }
    
    public void PlayDieEffect()
    {
        switch (slimeType)
        {
            case SlimeType.Big:
                SoundManager.instance.PlaySFX(SfxEffect.SlimeDie);
                break;
            case SlimeType.Normal:
                SoundManager.instance.PlaySFX(SfxEffect.SlimeDieMid);
                break;
            case SlimeType.Small:
                SoundManager.instance.PlaySFX(SfxEffect.SlimeDieSmall);
                break;
        }
    }

    public void PlayBiteEffect()
    {
        switch (slimeType)
        {
            case SlimeType.Big:
                SoundManager.instance.PlaySFX(SfxEffect.SlimeBite);
                break;
            case SlimeType.Normal:
                SoundManager.instance.PlaySFX(SfxEffect.SlimeBiteMid);
                break;
            case SlimeType.Small:
                SoundManager.instance.PlaySFX(SfxEffect.SlimeBiteSmall);
                break;
        }
    }
}
