using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{

    #region Movement
    [SerializeField] public float idleStateTime;
    [SerializeField] public float battleNoDetectionTime;
    
    #endregion
    
    #region States

    public SkeletonStateIdle stateIdle { get; private set; }
    public SkeletonStateMove stateMove { get; private set; }
    public SkeletonStateBattle stateBattle { get; private set; }
    public SkeletonStateAttack stateAttack { get; private set; }

    #endregion

    #region PlayerDetection

    [Header("Player Detection")]
    [SerializeField] public float detectionDistance;
    [SerializeField] public LayerMask whatIsPlayer;
    [SerializeField] public Transform playerCheck;
    [SerializeField] public float moveBattleMultiplayer;
    [SerializeField] public Player player;
    [SerializeField] public float attackDistance;
    
    #endregion

    public override void Awake()
    {
        base.Awake();

        stateMachine = new EnemyStateMachine();
        stateIdle = new SkeletonStateIdle(this, stateMachine, "Idle", this);
        stateMove = new SkeletonStateMove(this, stateMachine, "Move", this);
        stateBattle = new SkeletonStateBattle(this, stateMachine, "Battle", this);
        stateAttack = new SkeletonStateAttack(this, stateMachine, "Attack", this);
    }

    public override void Start()
    {
        base.Start();
        
        stateMachine.Initialize(stateIdle);
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + detectionDistance*faceDir, playerCheck.position.y));
    }

    public RaycastHit2D PlayerDetectionRaycast()
    {
        return Physics2D.Raycast(playerCheck.position, Vector2.right, detectionDistance*faceDir, whatIsPlayer);
    }
    
}
