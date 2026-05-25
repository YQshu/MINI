public class Player_MoveState : PlayerState
{
    public Player_MoveState(Player player, StateMachine stateMachine, string animBoolName)
        : base(player, stateMachine, animBoolName) { }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(moveInput.x * player.moveSpeed, rb.velocity.y);

        if (moveInput.x == 0)
            stateMachine.ChangeState(player.idleState);
        else if (input.Player.Jump.WasPressedThisFrame() && player.groundDetected)
            stateMachine.ChangeState(player.jumpState);
        else if (!player.groundDetected)
            stateMachine.ChangeState(player.fallState);
    }
}
