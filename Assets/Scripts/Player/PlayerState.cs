using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    public Player player;
    public PlayerStateMachine stateMachine;
    public string anim;

    public float stateTime;
    public bool finishedAnimation;
    
    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _anim)
    {
        player = _player;
        stateMachine = _stateMachine;
        anim = _anim;
        stateTime = 0;
    }
    
    public virtual void Enter()
    {
        finishedAnimation = false;
        player.animator.SetBool(anim, true);
    }
    
    public virtual void Update()
    {
        if (stateTime >= 0)
        {
            stateTime -= Time.deltaTime;
        }
    }

    public virtual void Exit()
    {
        player.animator.SetBool(anim, false);
    }

    public void FinishedAnimationsTrigger()
    {
        finishedAnimation = true;
    }
}
