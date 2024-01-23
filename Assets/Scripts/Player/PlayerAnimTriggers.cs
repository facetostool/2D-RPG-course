using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimTriggers : MonoBehaviour
{
    private Player _player;
    
    void Start()
    {
        _player = GetComponentInParent<Player>();
    }
    
    void Update()
    {
        
    }

    public void CancelAnimationTrigger()
    {
        _player.stateMachine.currentState.StopAnimationsTrigger();
    }
}
