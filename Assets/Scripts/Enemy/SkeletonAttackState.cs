using UnityEngine;

public class SkeletonAttackState : EnemyState
{
    EnemySkeleton enemy;

    public SkeletonAttackState(Enemy _enemyBase, EnemyStatemackine _stateMachine, string _animBoolName, EnemySkeleton enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

        enemy.lastTimeAttack = Time.time;
    }

    public override void Update()
    {
        base.Update();
        enemy.ZeroVelocity();

        if (triggerCalled)
        {
            statemackine.ChangeState(enemy.battleState);
        }
    }
}
