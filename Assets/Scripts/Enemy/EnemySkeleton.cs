using UnityEngine;

public class EnemySkeleton : Enemy
{

    #region States
    public SkeletonIdleState idleState { get; private set; }
    public SkeletonMoveState moveState { get; private set; }
    public SkeletonBattleState battleState { get; private set; }
    public SkeletonAttackState attackState { get; private set; }
    public SkeletonStunState stunState { get; private set; }
    public SkeletonDeadState deadState { get; private set; }

    #endregion


    protected override void Awake()
    {
        base.Awake();
        idleState = new SkeletonIdleState(this, statemackine, "Idle", this);
        moveState = new SkeletonMoveState(this, statemackine, "Move", this);
        battleState = new SkeletonBattleState(this, statemackine, "Move", this);
        attackState = new SkeletonAttackState(this, statemackine, "Attack", this);
        stunState = new SkeletonStunState(this, statemackine, "Stun", this);
        deadState = new SkeletonDeadState(this, statemackine, "Idle", this);
    }

    protected override void Start()
    {
        base.Start();
        statemackine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.M))
        {
            statemackine.ChangeState(stunState);
        }
    }
    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            statemackine.ChangeState(stunState);
            return true;
        }
        return false;
    }
    public override void Die()
    {
        base.Die();
        statemackine.ChangeState(deadState);
    }
}
