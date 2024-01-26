using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{

    #region States

    public SkeletonStateIdle stateIdle { get; private set; }
    public SkeletonStateMove stateMove { get; private set; }

    #endregion

    public override void Awake()
    {
        base.Awake();

        stateMachine = new EnemyStateMachine();
        stateIdle = new SkeletonStateIdle(this, stateMachine, "Idle", this);
        stateMove = new SkeletonStateMove(this, stateMachine, "Move", this);
    }

    public override void Start()
    {
        base.Start();
        
        stateMachine.Initialize(stateIdle);
    }
}
