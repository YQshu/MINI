using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private Transform player;
    private EnemySkeleton enemy;
    private int moveDir;
    public SkeletonBattleState(Enemy _enemyBase, EnemyStatemackine _stateMachine, string _animBoolName, EnemySkeleton enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        player = PlayerManager.Instance.player.transform;

        if(player.GetComponent<PlayerStats>().isDead)
        {
            statemackine.ChangeState(enemy.moveState);
        }

    }
    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;

            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
                statemackine.ChangeState(enemy.attackState);
            }
        }
        else
        {
            if (stateTimer < 0 || !enemy.IsGroundeDetected())
            {
                statemackine.ChangeState(enemy.idleState);
            }
        }


        if (player.position.x > enemy.transform.position.x)
        {
            moveDir = 1;
        }
        else if (player.position.x < enemy.transform.position.x)
        {
            moveDir = -1;
        }
        enemy.SetVelocity(enemy.moveSpeed * moveDir,enemy.rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool canAttack()
    {
        if (Time.time >= enemy.lastTimeAttack + enemy.attackCooldown)
        {
            enemy.attackCooldown = Random.Range(enemy.minAttackCooldown, enemy.maxAttackCooldown);
            enemy.lastTimeAttack = Time.time;
            return true;
        }
        return false;
    }

}
