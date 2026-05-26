using UnityEngine;

public class Player_CollectState : PlayerState
{
    public float collectDuration = 0.5f;

    private float collectTimer;

    public Player_CollectState(Player player, StateMachine stateMachine, string animBoolName)
        : base(player, stateMachine, animBoolName) { }

    public override void Enter()
    {
        base.Enter();

        collectTimer = collectDuration;

        player.SetVelocity(0, 0);
        player.input.Disable();
    }

    public override void Update()
    {
        base.Update();

        collectTimer -= Time.deltaTime;

        if (collectTimer <= 0)
        {
            if (player.groundDetected)
            {
                if (moveInput.x != 0)
                    stateMachine.ChangeState(player.moveState);
                else
                    stateMachine.ChangeState(player.idleState);
            }
            else
            {
                stateMachine.ChangeState(player.fallState);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();

        player.input.Enable();
    }
}
