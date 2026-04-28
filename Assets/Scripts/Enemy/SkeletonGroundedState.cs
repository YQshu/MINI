using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonGroundedState : EnemyState
{
    protected EnemySkeleton enemy;

    protected Transform player;


    public SkeletonGroundedState(Enemy _enemyBase, EnemyStatemackine _stateMachine, string _animBoolName , EnemySkeleton enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.Instance.player.transform;
    }
    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected() || Vector2.Distance(enemy.transform.position , player.transform.position)  <  1 )
        {
            statemackine.ChangeState(enemy.battleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

}
