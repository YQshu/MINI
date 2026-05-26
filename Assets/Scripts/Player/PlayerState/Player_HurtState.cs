using UnityEngine;

public class Player_HurtState : PlayerState
{
    [Header("Hurt details")]
    public float hurtDuration = 0.4f;
    public Vector2 knockbackForce = new Vector2(5f, 3f);

    private float hurtTimer;

    public Player_HurtState(Player player, StateMachine stateMachine, string animBoolName)
        : base(player, stateMachine, animBoolName) { }

    public override void Enter()
    {
        base.Enter();

        hurtTimer = hurtDuration;

        // Knockback in opposite direction of facing
        float knockbackDir = -player.facingDir;
        player.SetVelocity(knockbackDir * knockbackForce.x, knockbackForce.y);
    }

    public override void Update()
    {
        base.Update();

        hurtTimer -= Time.deltaTime;

        if (hurtTimer <= 0)
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
}
