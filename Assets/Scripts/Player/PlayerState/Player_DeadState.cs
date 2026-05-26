public class Player_DeadState : PlayerState
{
    public Player_DeadState(Player player, StateMachine stateMachine, string animBoolName)
        : base(player, stateMachine, animBoolName) { }

    public override void Enter()
    {
        base.Enter();

        // Disable player input on death
        player.input.Disable();
    }

    public override void Update()
    {
        base.Update();

        // Zero out velocity so the body stays still
        player.SetVelocity(0, rb.velocity.y);
    }
}
