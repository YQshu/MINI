using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.Skill.clone.CreateClone(player.transform);

        stateTimer = player.dashDuration;
        player.stats.MakeInvincible(true);
    }

    public override void Exit()
    {
        base.Exit();

        player.SetVelocity(0,player.rb.velocity.y);
        player.stats.MakeInvincible(false);
    }

    public override void Update()
    {
        base.Update();

        if(!player.IsGroundeDetected() && player.IsWallDetected())
        {
            stateMachine.ChangeState(player.wallSlideState);
        }

        player.SetVelocity(player.dashSpeed * player.dashDir,0);

        if (stateTimer < 0)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
