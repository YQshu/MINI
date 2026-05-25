public class Player_IdleState : PlayerState
{
    public Player_IdleState(Player player, StateMachine stateMachine, string animBoolName)
        : base(player, stateMachine, animBoolName) { }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocity(0, rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        if (moveInput.x != 0)
            stateMachine.ChangeState(player.moveState);
        else if (input.Player.Jump.WasPressedThisFrame() && player.groundDetected)
            stateMachine.ChangeState(player.jumpState);
        else if (!player.groundDetected)
            stateMachine.ChangeState(player.fallState);
    }
}
