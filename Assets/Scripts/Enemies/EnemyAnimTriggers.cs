using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimTriggers : MonoBehaviour
{
    private Enemy _enemy;
    
    void Start()
    {
        _enemy = GetComponentInParent<Enemy>();
    }
    
    void Update()
    {
        
    }

    public void CancelAnimationTrigger()
    {
        _enemy.stateMachine.currentState.StopAnimationsTrigger();
    }
    
    public void OnHitAttackTrigger()
    {
        _enemy.OnHitAttackTrigger();
    }

    public void OpenCounterAttackWindow()
    {
        _enemy.OpenCounterAttackWindow();
        Invoke(nameof(CloseCounterAttackWindow), 1f);
    }
    
    public void CloseCounterAttackWindow()
    {
        _enemy.CloseCounterAttackWindow();
    }
}
