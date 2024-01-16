using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    public Player player;
    public PlayerStateMachine stateMachine;
    public string anim;

    public float stateTime;
    
    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _anim)
    {
        player = _player;
        stateMachine = _stateMachine;
        anim = _anim;
    }
    
    public virtual void Enter()
    {
        player.animator.SetBool(anim, true);
    }
    
    public virtual void Update()
    {
        
    }

    public virtual void Exit()
    {
        player.animator.SetBool(anim, false);
    }
}
