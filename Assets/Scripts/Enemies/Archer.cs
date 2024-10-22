using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Enemy
{

    #region Movement
    [Header("Archer info")]
    [SerializeField] public float idleStateTime;
    [SerializeField] public float battleNoDetectionTime;
    [SerializeField] public float jumpForce;
    #endregion

    #region Bow shot
    [Header("Bow shot")]
    [SerializeField] public GameObject arrowPrefab;
    [SerializeField] public Transform arrowSpawnPoint;
    [SerializeField] public float arrowSpeed;
    #endregion
    
    #region States

    public ArcherStateIdle stateIdle { get; private set; }
    public ArcherStateMove stateMove { get; private set; }
    public ArcherStateBattle stateBattle { get; private set; }
    public ArcherStateAttack stateAttack { get; private set; }
    public ArcherStateDead stateDead { get; private set; }
    public ArcherStateJump stateJump { get; private set; }
    public ArcherStateAir stateAir { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new EnemyStateMachine();
        stateIdle = new ArcherStateIdle(this, stateMachine, "Idle", this);
        stateMove = new ArcherStateMove(this, stateMachine, "Move", this);
        stateBattle = new ArcherStateBattle(this, stateMachine, "Move", this);
        stateAttack = new ArcherStateAttack(this, stateMachine, "Attack", this);
        stateJump = new ArcherStateJump(this, stateMachine, "Air", this);
        stateDead = new ArcherStateDead(this, stateMachine, "Die", this);
        stateAir = new ArcherStateAir(this, stateMachine, "Air", this);
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
    
    public override void Die()
    {
        base.Die();
        
        stateMachine.ChangeState(stateDead);
    }

    public void SpawnArrow()
    {
        GameObject arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, Quaternion.identity);
        arrow.GetComponent<ArrowController>().Setup(10, arrowSpeed, faceDir);
    }
}
