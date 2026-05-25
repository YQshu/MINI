using UnityEngine;
using PlayerInputSet = PlayerInput.PlayerInput;

public abstract class PlayerState : EntityState
{
    protected Player player;

    protected PlayerInputSet input;

    protected Vector2 moveInput;

    public PlayerState(Player player, StateMachine stateMachine, string animBoolName)
        : base(stateMachine, animBoolName)
    {
        this.player = player;

        this.anim = player.anim;
        this.rb = player.rb;
        this.input = player.input;
    }

    public override void Update()
    {
        base.Update();

        moveInput = player.moveInput;
    }

    public override void UpdateAnimationParameters()
    {
        base.UpdateAnimationParameters();

        anim.SetFloat("yVelocity", rb.velocity.y);
    }
}
