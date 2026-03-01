using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected EnemyStatemackine statemackine;
    protected Enemy enemyBase;
    protected Rigidbody2D rb;

    protected bool triggerCalled;
    private string animBoolName;
    protected float stateTimer;

    public EnemyState(Enemy _enemyBase, EnemyStatemackine _stateMachine, string _animBoolName)
    {
        this.enemyBase = _enemyBase;
        this.statemackine = _stateMachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Update()
    {
        stateTimer = stateTimer - Time.deltaTime;
    }


    public virtual void Enter()
    { 
        triggerCalled = false;
        rb = enemyBase.rb;
        enemyBase.anim.SetBool(animBoolName, true);
    }

    public virtual void Exit()
    {
        triggerCalled = true;
        enemyBase.anim.SetBool(animBoolName, false);
        enemyBase.AssignLastAnimName(animBoolName);
    }

    public virtual void AnimationFinishedTriggers()
    {
        triggerCalled = true;
    }
}