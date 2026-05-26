using UnityEngine;
using PlayerInputSet = PlayerInput.PlayerInput;

public class Player : Entity
{
    public PlayerInputSet input { get; private set; }

    public Vector2 moveInput { get; private set; }

    #region 玩家状态
    public Player_IdleState idleState { get; private set; }
    public Player_MoveState moveState { get; private set; }
    public Player_JumpState jumpState { get; private set; }
    public Player_FallState fallState { get; private set; }
    public Player_HurtState hurtState { get; private set; }
    public Player_DeadState deadState { get; private set; }
    public Player_CollectState collectState { get; private set; }
    #endregion

    #region 移动相关参数
    [Header("Movement details")]
    public float moveSpeed = 5f;
    public float jumpForce = 8f;

    [Header("Animation scale")]
    public float jumpSpriteScale = 1.5f;
    #endregion

    protected override void Awake()
    {
        base.Awake();

        input = new PlayerInputSet();

        idleState = new Player_IdleState(this, stateMachine, "Idle");
        moveState = new Player_MoveState(this, stateMachine, "Move");
        jumpState = new Player_JumpState(this, stateMachine, "Jump");
        fallState = new Player_FallState(this, stateMachine, "Fall");
        hurtState = new Player_HurtState(this, stateMachine, "Hurt");
        deadState = new Player_DeadState(this, stateMachine, "Dead");
        collectState = new Player_CollectState(this, stateMachine, "Collect");
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    private void OnEnable()
    {
        input.Enable();

        input.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Player.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    private void OnDisable()
    {
        input.Disable();
    }

    public Vector2 respawnPosition { get; set; }

    #region 受伤逻辑
    [Header("Hurt details")]
    public float hurtCooldown = 0.5f;
    private float lastHurtTime;

    public override void TakeDamage(int damage)
    {
        if (isDead) return;
        if (Time.time - lastHurtTime < hurtCooldown) return;

        lastHurtTime = Time.time;
        base.TakeDamage(damage);

        if (isDead)
            stateMachine.ChangeState(deadState);
        else
            stateMachine.ChangeState(hurtState);
    }
    #endregion
}
