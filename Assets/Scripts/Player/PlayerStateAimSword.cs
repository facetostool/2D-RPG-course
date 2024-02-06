using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateAimSword : PlayerState
{
    public PlayerStateAimSword(Player _player, PlayerStateMachine _stateMachine, string _anim) : base(_player, _stateMachine, _anim)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        
        player.SetVelocity(0,0);
        
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            player.skills.throwSword.Use();
            stateMachine.ChangeState(player.stateIdle);
            return;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
