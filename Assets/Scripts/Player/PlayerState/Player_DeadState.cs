using UnityEngine;

public class Player_DeadState : PlayerState
{
    public Player_DeadState(Player player, StateMachine stateMachine, string animBoolName)
        : base(player, stateMachine, animBoolName) { }

    public override void Enter()
    {
        base.Enter();

        // Disable player input on death
        player.input.Disable();

        // Stop movement
        player.SetVelocity(0, 0);

        // Show death screen
        var deathScreen = Object.FindObjectOfType<DeathScreenManager>();
        if (deathScreen != null)
        {
            deathScreen.ShowDeathScreen();
        }
    }

    public override void Update()
    {
        base.Update();

        // Keep velocity zeroed
        player.SetVelocity(0, rb.velocity.y);
    }
}
