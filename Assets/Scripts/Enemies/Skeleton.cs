using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{

    #region Movement
    [Header("Skeleton info")]
    [SerializeField] public float idleStateTime;
    [SerializeField] public float battleNoDetectionTime;
    #endregion
    
    #region States

    public SkeletonStateIdle stateIdle { get; private set; }
    public SkeletonStateMove stateMove { get; private set; }
    public SkeletonStateBattle stateBattle { get; private set; }
    public SkeletonStateAttack stateAttack { get; private set; }
    public SkeletonStateStunned stateStunned { get; private set; }
    public SkeletonStateDead stateDead { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new EnemyStateMachine();
        stateIdle = new SkeletonStateIdle(this, stateMachine, "Idle", this);
        stateMove = new SkeletonStateMove(this, stateMachine, "Move", this);
        stateBattle = new SkeletonStateBattle(this, stateMachine, "Battle", this);
        stateAttack = new SkeletonStateAttack(this, stateMachine, "Attack", this);
        stateStunned = new SkeletonStateStunned(this, stateMachine, "Stunned", this);
        stateDead = new SkeletonStateDead(this, stateMachine, "Die", this);
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
