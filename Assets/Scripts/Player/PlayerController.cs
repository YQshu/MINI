using UnityEngine;

/// <summary>
/// 实现人物移动（支持二段跳）
/// </summary>
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private InputSystemController inputController;

    [Header("移动设置")]
    [SerializeField] private float speed = 5f;

    [Header("跳跃设置")]
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private int maxJumps = 2;  // 最大跳跃次数

    [Header("地面检测设置")]
    [SerializeField] private Transform groundCheckPoint;  // 地面检测点
    [SerializeField] private float groundCheckRadius = 0.2f;  // 检测半径
    [SerializeField] private LayerMask groundLayer;  // 地面图层

    private Vector2 movementInput;  // 移动输入

    //跳跃状态
    private int jumpsLeft;  // 剩余跳跃次数
    private bool isGrounded;  // 当前是否在地面
    private bool wasGroundedPreviousFrame; // 上一帧是否在地面

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        OnLand();
    }
    
    private void Start()
    {
        inputController = InputSystemController.Instance;
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Update()
    {
        Jump();

        Interact();
        GroundCheckUpdate();
    }

    private void GroundCheckUpdate()
    {
        // 更新地面状态
        wasGroundedPreviousFrame = isGrounded;
        isGrounded = IsGrounded();

        // 落地检测：从空中变为地面时重置跳跃次数
        if (!wasGroundedPreviousFrame && isGrounded)
        {
            OnLand();
        }
    }

    //控制移动
    private void Movement()
    {
        movementInput = inputController.GetMovementInput();
        rb.velocity = new Vector2(movementInput.x * speed, rb.velocity.y);
    }

    //控制跳跃
    private void Jump()
    {
        // 还有剩余跳跃次数才能跳
        if (inputController.GetPlayerJumpPressed() && jumpsLeft > 0)
        {
            // 执行跳跃
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            // 消耗一次跳跃机会
            jumpsLeft--;  

            // 可选：添加跳跃音效或特效
            Debug.Log($"跳跃！剩余跳跃次数: {jumpsLeft}, 是否在地面: {isGrounded}");
        }
    }

    //控制交互
    private void Interact()
    {
        if (inputController.GetPlayerConfirmPressed())
        {
            GameModeManager.Instance?.ChangeGameMode();
        }
    }

    //落地处理
    private void OnLand()
    {
        jumpsLeft = maxJumps;
        Debug.Log($"落地！跳跃次数已重置: {jumpsLeft}");
    }

    //检测是否在地面
    private bool IsGrounded()
    {
        if (groundCheckPoint == null) return false;

        // 球形检测地面
        Collider2D colliders = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);

        return colliders != null;
    }

    // 可视化调试（可在Scene视图看到检测范围）
    private void OnDrawGizmos()
    {
        if (groundCheckPoint != null)
        {
            // 绘制球形检测范围，绿色表示在地面，红色表示在空中
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
        }
    }
}