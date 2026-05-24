using UnityEngine;

public class PushableBox : MonoBehaviour
{
    public float pushSpeed = 3f;
    private Rigidbody2D rb;
    private bool isBeingPushed = false;
    private Transform pusher;

    void Awake() => rb = GetComponent<Rigidbody2D>();

    void OnCollisionStay2D(Collision2D collision)
    {
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
        if (isBeingPushed && pusher != null)
        {
            float moveDir = Mathf.Sign(transform.position.x - pusher.position.x);
            rb.velocity = new Vector2(moveDir * pushSpeed, rb.velocity.y);
        }
    }
}
