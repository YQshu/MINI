using UnityEngine;

public class PushableBox : MonoBehaviour
{
    public float pushSpeed = 3f;

    private Rigidbody2D rb;
    private bool isBeingPushed = false;
    private Transform pusher;

    // 是否被锁定（由压力板控制）
    [HideInInspector] public bool isLocked = false;

    void Awake() => rb = GetComponent<Rigidbody2D>();

    void OnCollisionStay2D(Collision2D collision)
    {
        // 被锁定时不能推动
        if (isLocked) return;

        if (collision.collider.CompareTag("Player"))
        {
            pusher = collision.transform;
            isBeingPushed = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            isBeingPushed = false;
            pusher = null;
        }
    }

    void FixedUpdate()
    {
        // 被锁定时不能移动
        if (isLocked)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (isBeingPushed && pusher != null)
        {
            float moveDir = Mathf.Sign(transform.position.x - pusher.position.x);
            rb.velocity = new Vector2(moveDir * pushSpeed, rb.velocity.y);
        }
    }

    // 供压力板调用的锁定方法
    public void Lock()
    {
        isLocked = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        rb.velocity = Vector2.zero;
    }
}