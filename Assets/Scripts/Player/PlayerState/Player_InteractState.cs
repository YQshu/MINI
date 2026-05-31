using UnityEngine;

public class Player_InteractState : PlayerState
{
    public float interactDuration = 0.5f;

    private float interactTimer;

    public Player_InteractState(Player player, StateMachine stateMachine, string animBoolName)
        : base(player, stateMachine, animBoolName) { }

    public override void Enter()
    {
        base.Enter();

        interactTimer = interactDuration;

        player.SetVelocity(0, 0);
        player.input.Disable();
    }

    public override void Update()
    {
        base.Update();

        interactTimer -= Time.deltaTime;

        if (interactTimer <= 0)
        {
            player.transform.position = player.portalTargetPosition;

            player.input.Enable();
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        player.input.Enable();
    }
}