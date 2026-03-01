using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] protected LayerMask whatIsPlayer;
    [Header("Stunned info")]
    public float stunDuration;
    public Vector2 stunDirection;
    protected bool CanbeStunned;
    [SerializeField] protected GameObject counterImage;


    [Header("Move info")]
    public float moveSpeed;
    public float idleTime;
    public float battleTime;
    private float defaultMoveSpeed;

    [Header("Attack info")]
    public float attackDistance;
    public float attackCooldown;
    public float minAttackCooldown;
    public float maxAttackCooldown;
    [HideInInspector] public float lastTimeAttack;

    public EnemyStatemackine statemackine {  get; private set; }
    public string lastAnimBoolName { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        statemackine = new EnemyStatemackine();
        defaultMoveSpeed = moveSpeed;
    }


    protected override void Update()
    {
        base.Update();
        statemackine.currentState.Update();
    }
    public virtual void AssignLastAnimName(string _animBoolName)
    {
        lastAnimBoolName = _animBoolName;
    }

    public override void SlowEntityBy(float _slowPercentage, float _slowduration)
    {
        anim.speed =anim.speed * (1 - _slowPercentage);
        moveSpeed = moveSpeed * (1 - _slowPercentage);
        Invoke("ReturnDefaultSpeed", _slowduration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();
        moveSpeed = defaultMoveSpeed;
    }

    public virtual void FreezeTime(bool _timeFrozen)
    {
        if (_timeFrozen)
        {
            moveSpeed = 0;
            anim.speed = 0;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
            anim.speed = 1;
        }
    }

    public virtual void FreezeTimeFor(float _duration) => StartCoroutine(FreezeTimeCoroutine(_duration));


    protected virtual IEnumerator FreezeTimeCoroutine(float _seconds)
    {
        FreezeTime(true);

        yield return new WaitForSeconds(_seconds);

        FreezeTime(false); 
    }

    #region Counter Attack Window
    public virtual void OpenCounterAttackWindows()
    {
        CanbeStunned = true;
        counterImage.SetActive(true);
    }

    public virtual void CloseCounterAttackWindows()
    {
        CanbeStunned = false;   
        counterImage.SetActive(false); 
    }

    #endregion
    public virtual bool CanBeStunned()
    {
        if (CanbeStunned)
        {
            CloseCounterAttackWindows();
            return true;
        }
        return false;
    }

    public virtual void AnimationFinshedTrigger() => statemackine.currentState.AnimationFinishedTriggers();

    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallcheck.position,Vector2.right * facingDir , 50, whatIsPlayer);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDistance * facingDir, transform.position.y));
    }

}