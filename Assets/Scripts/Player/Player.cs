using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : Entity
{
    [Header("Attack Details")]
    public Vector2[] attackMovement;
    public float counterAttackDuration = .2f;


    public bool isBusy {  get; private set; }
    [Header("Move info")]
    public float moveSpeed = 12f;
    public float jumpForce;
    public float swordReturnImpact;
    private float defaultMoveSpeed;
    private float defaultJumpForce;
    private float defaultDashSpeed;

    [Header("Dash info")]
    public float dashSpeed;
    public float dashDuration;
    public float dashDir {  get; private set; }

    public SkillManager Skill {  get; private set;  }
    public GameObject sword {  get; private set; }

    #region States
    public PlayerStateMachine StateMachine {  get; private set; }
    public PlayeridleState idleState{ get; private set; }
    public PlayerMoveState moveState{ get; private set; }
    public PlayerJumpState jumpState{ get; private set; }
    public PlayerAirState airState{ get; private set; }
    public PlayerDashState dashState{ get; private set; }
    public PlayerWallSlideState wallSlideState{ get; private set; }
    public PlayerWallJump wallJumpState{ get; private set; }
    public PlayerPrimaryAttack PrimaryAttack {  get; private set; }
    public PlayerCounterAttack counterAttack { get; private set; }
    public PlayerAnimSwordState aimSword { get; private set; }
    public PlayerCatchSwordState catchSword { get; private set; }
    public PlayerDeadState deadState{ get; private set; }

    #endregion

     protected override void Awake()
    {
        base.Awake();
        StateMachine = new PlayerStateMachine();

        counterAttack = new PlayerCounterAttack(this,StateMachine, "CounterAttack");
        idleState = new PlayeridleState(this, StateMachine, "Idle");
        moveState = new PlayerMoveState(this, StateMachine, "Move");
        jumpState = new PlayerJumpState(this, StateMachine, "Jump");
        airState = new PlayerAirState(this, StateMachine, "Jump");
        dashState = new PlayerDashState(this, StateMachine, "Dash");
        wallSlideState = new PlayerWallSlideState(this, StateMachine, "WallSlide");
        wallJumpState = new PlayerWallJump(this, StateMachine, "Jump");
        PrimaryAttack = new PlayerPrimaryAttack(this, StateMachine, "Attack");
        aimSword = new PlayerAnimSwordState(this, StateMachine, "AimSword");
        catchSword = new PlayerCatchSwordState(this, StateMachine, "CatchSword");
        deadState = new PlayerDeadState(this, StateMachine, "Die");
    }

    protected override void Start()
    {
        base.Start();
        Skill = SkillManager.instance;
        StateMachine.Initialize(idleState);
        defaultJumpForce = jumpForce;
        defaultMoveSpeed = moveSpeed;
        defaultDashSpeed = dashSpeed;
    }


    protected override void Update()
    {
        if(Time.timeScale == 0)
        {
            return;
        }

        base.Update();
        StateMachine.currentState.Update();
        CheckForDashInput();

        if (Input.GetKeyDown(KeyCode.R))
        {
            Inventory.instance.UseFlask();
        }
    }

    public override void SlowEntityBy(float _slowPercentage, float _slowduration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercentage);
        jumpForce = jumpForce * (1 - _slowPercentage);
        dashSpeed = dashSpeed * (1 - _slowPercentage);
        anim.speed = anim.speed * (1 - _slowPercentage);

        Invoke("ReturnDefaultSpeed", _slowduration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;
    }

    public void AssignNewSword(GameObject _newsword)
    {
        sword = _newsword;
    }

    public void CatchTheSword()
    { 
        StateMachine.ChangeState(catchSword);
        Destroy(sword);
    }


    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;

        yield return new WaitForSeconds(_seconds);

        isBusy = false; 
    }

    public void AnimationTrigger() => StateMachine.currentState.AnimaitonFinishTrigger();


    public void CheckForDashInput()
    {
        if(IsWallDetected())
        {
            return;
        }


        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash.CanUseSkill())
        {
            dashDir = Input.GetAxisRaw("Horizontal");

            if (dashDir == 0)
            {
                dashDir = facingDir;
            }

            StateMachine.ChangeState(dashState);
        }
    }
    public override void Die()
    {
        base.Die();
        StateMachine.ChangeState(deadState);
    }
    protected override void SetupZeroKnockBackPower()
    {
        knockvackPower = new Vector2(0, 0);
    }
}
