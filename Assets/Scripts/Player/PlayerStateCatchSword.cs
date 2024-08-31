using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateCatchSword : PlayerState
{
    public PlayerStateCatchSword(Player _player, PlayerStateMachine _stateMachine, string _anim) : base(_player, _stateMachine, _anim)
    {
    }

    public override void Enter()
    {
        base.Enter();
        SoundManager.instance.PlaySFX(SfxEffect.SwordThrow2);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
