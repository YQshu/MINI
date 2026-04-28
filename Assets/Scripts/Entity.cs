using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour //实体部分的基类包含了一些基本的属性和方法
{
    [Header("Knock info")]         //击退相关信息
    [SerializeField] protected Vector2 knockvackPower;
    [SerializeField] protected float KnockBackTime;
    protected bool isKnocked;

    [Header("Collision info")]    //碰撞相关信息包含地面检测、墙检测、攻击检测等
    public Transform attackCheck;
    public float attackCheckRiadius;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallcheck;
    [SerializeField] protected float wallcheckDistance;
    [SerializeField] protected LayerMask whatIsGround;

    public int knockbackDir {  get; private set; }
    //组件
    #region Component  
    public Animator anim { get; private set; }  
    public Rigidbody2D rb { get; private set; }
    public EntityFx fx { get; private set; }
    public Characterstats stats { get; private set; }
    public CapsuleCollider2D cd { get; private set; }

    #endregion

    public int facingDir { get; private set; } = 1;
    protected bool facingRight = true;

    public System.Action onFipped;
    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        fx = GetComponentInChildren<EntityFx>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<Characterstats>();
        cd = GetComponent<CapsuleCollider2D>();
    }

    protected virtual void Update()
    {
        
    }

    public virtual void SlowEntityBy(float _slowPercentage , float _slowduration)
    {

    }

    protected virtual void ReturnDefaultSpeed()
    {
        anim.speed = 1;
    }

    public virtual void DamageImpact()
    {
        StartCoroutine("HitKnockBack");
        //Debug.Log(gameObject.name + " was damaged");
    }

    public virtual void SetupKnockbackDir(Transform _damageDirection)
    {
        if(_damageDirection.position.x > transform.position.x)
        {
            knockbackDir = -1;
        }else if(_damageDirection.position.x < transform.position.x)
        {
            knockbackDir = 1;
        }
    }

    public void SetupKnockbackPower(Vector2 _knockbackPower) => knockvackPower = _knockbackPower;

    protected virtual IEnumerator HitKnockBack()
    {
        isKnocked = true;

        rb.velocity = new Vector2(knockvackPower.x * knockbackDir,knockvackPower.y );

        yield return new WaitForSeconds(KnockBackTime);

        isKnocked= false;
        SetupZeroKnockBackPower();
    }

    protected virtual void SetupZeroKnockBackPower()
    {

    }


    #region Collision
    public virtual bool IsGroundeDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallcheck.position, Vector2.right * facingDir, wallcheckDistance, whatIsGround);
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallcheck.position, new Vector3(wallcheck.position.x + wallcheckDistance * facingDir, wallcheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRiadius);
    }
    #endregion
    #region Filp
    public void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);

        if (onFipped != null)
        {
            onFipped();
        }
    }

    public void FilpController(float _x)
    {
        if (_x > 0 && !facingRight)
        {
            Flip();
        }
        else if (_x < 0 && facingRight)
        {
            Flip();
        }
    }
    #endregion
    #region Velocity
    public void ZeroVelocity()
    {
        if (isKnocked)
        {
            return;
        }
        rb.velocity = new Vector2(0, 0);
    }
    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        if (isKnocked)
        {
            return;
        }
        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FilpController(_xVelocity);
    }
    #endregion

    public virtual void Die()
    {

    }
}
