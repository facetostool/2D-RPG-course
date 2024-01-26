using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public EnemyStateMachine stateMachine;
    
    public override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        
        stateMachine.currentState.Update();
    }
}
