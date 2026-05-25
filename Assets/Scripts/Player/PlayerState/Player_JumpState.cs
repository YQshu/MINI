using UnityEngine;

public class Player_JumpState : PlayerState
{
    public Player_JumpState(Player player, StateMachine stateMachine, string animBoolName)
        : base(player, stateMachine, animBoolName) { }

    public override void Enter()
    {
        base.Enter();
        rb.velocity = new Vector2(rb.velocity.x, player.jumpForce);
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

        if (rb.velocity.y <= 0)
            stateMachine.ChangeState(player.fallState);
    }
}
