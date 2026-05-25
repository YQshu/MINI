using UnityEngine;

public class Player_FallState : PlayerState
{
    public Player_FallState(Player player, StateMachine stateMachine, string animBoolName)
        : base(player, stateMachine, animBoolName) { }

    public override void Enter()
    {
        base.Enter();
        anim.transform.localScale = Vector3.one * player.jumpSpriteScale;
    }

    public override void Exit()
    {
        base.Exit();
        anim.transform.localScale = Vector3.one;
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(moveInput.x * player.moveSpeed, rb.velocity.y);

        if (player.groundDetected)
        {
            if (moveInput.x != 0)
                stateMachine.ChangeState(player.moveState);
            else
                stateMachine.ChangeState(player.idleState);
        }
    }
}
