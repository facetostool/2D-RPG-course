using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateIdle : PlayerStateGrounded
{
    public PlayerStateIdle(Player _player, PlayerStateMachine _stateMachine, string _anim): base(_player, _stateMachine, _anim)
    {
        
    }
        
    public override void Enter()
    {
        base.Enter();
    }
    
    public override void Update()
    {
        base.Update();
        
        if (player.moveVector.x != 0)
        {
            stateMachine.ChangeState(player.stateMove);
        }
    }
    
    public override void Exit()
    {
        base.Exit();
    }
}
