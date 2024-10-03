using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{

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

    #region PlayerDetection

    [Header("Player Detection")]
    [SerializeField] public float detectionDistance;
    [SerializeField] public LayerMask whatIsPlayer;
    [SerializeField] public Transform playerCheck;
    [SerializeField] public float moveBattleMultiplayer;
    [SerializeField] public float attackDistance;
    
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
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + detectionDistance*faceDir, playerCheck.position.y));
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + attackDistance*faceDir, playerCheck.position.y));
    }

    public RaycastHit2D PlayerDetectionRaycast()
    {
        return Physics2D.Raycast(playerCheck.position, Vector2.right, detectionDistance*faceDir, whatIsPlayer);
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
}
