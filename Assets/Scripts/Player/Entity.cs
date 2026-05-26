using UnityEngine;

public class Entity : MonoBehaviour
{
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public StateMachine stateMachine { get; private set; }

    [Header("Health info")]
    [SerializeField] protected int maxHealth = 100;
    public int currentHealth { get; protected set; }
    public bool isDead { get; protected set; }

    [Header("Collision info")]
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected float groundCheckDistance = 0.3f;
    [SerializeField] protected float wallCheckDistance = 0.5f;
    [SerializeField] protected Transform wallCheck;

    public bool groundDetected { get; protected set; }
    public bool wallDetected { get; protected set; }
    public int facingDir { get; private set; } = 1;

    protected virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stateMachine = new StateMachine();
    }

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    public virtual void Die()
    {
        isDead = true;
    }

    protected virtual void Update()
    {
        stateMachine.UpdateActiveState();
        CheckCollision();
    }

    private void CheckCollision()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            Vector2 origin = new Vector2(transform.position.x, col.bounds.min.y);
            groundDetected = Physics2D.Raycast(origin, Vector2.down, groundCheckDistance, whatIsGround);
        }

        if (wallCheck != null)
        {
            wallDetected = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
        }
    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        rb.velocity = new Vector2(xVelocity, yVelocity);
        HandleFlip(xVelocity);
    }

    public void HandleFlip(float xVelocity)
    {
        if (xVelocity > 0 && facingDir == -1)
            Flip();
        else if (xVelocity < 0 && facingDir == 1)
            Flip();
    }

    protected void Flip()
    {
        facingDir *= -1;
        transform.Rotate(0, 180, 0);
    }

    protected virtual void OnDrawGizmos()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            Gizmos.color = Color.green;
            Vector2 origin = new Vector2(transform.position.x, col.bounds.min.y);
            Gizmos.DrawLine(origin, origin + Vector2.down * groundCheckDistance);
        }

        if (wallCheck != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + Vector3.right * facingDir * wallCheckDistance);
        }
    }
}
