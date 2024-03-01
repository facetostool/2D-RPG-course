using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateUltimate : PlayerState
{
    private bool isSkillUsed;
    private float defaultGravityScale;
    
    public PlayerStateUltimate(Player _player, PlayerStateMachine _stateMachine, string _anim) : base(_player, _stateMachine, _anim)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        defaultGravityScale = player.rb.gravityScale;
        player.rb.gravityScale = 0;
        isSkillUsed = false;

        stateTime = 1;
    }

    public override void Update()
    {
        base.Update();

        if (stateTime > 0)
        {
            player.rb.velocity = new Vector2(0, 5);
        }
        
        if (stateTime <= 0)
        {
            player.rb.velocity = new Vector2(0, -0.1f);
            if (!isSkillUsed)
            {
                player.skills.blackHole.Use();
                isSkillUsed = true;
            }
        }
    }

    public override void Exit()
    {
        base.Exit();

        player.rb.gravityScale = defaultGravityScale;
    }
}
