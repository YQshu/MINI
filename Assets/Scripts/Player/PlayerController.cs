using UnityEngine;

/// <summary>
/// 实现人物移动
/// </summary>
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    //移速
    [SerializeField] private float speed = 5f;

    //输入参数
    private Vector2 movementInput;
    private bool isInteracting;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Movement();
        Interact();
    }

    //控制移动
    private void Movement()
    {
        var input = InputSystemController.Instance;
        if (input == null) return;
    
        movementInput = input.GetMovementInput();
        rb.velocity = new Vector2(movementInput.x * speed, rb.velocity.y);
    }

    //控制交互
    private void Interact()
    {
        var input = InputSystemController.Instance;
        if (input == null) return;

        isInteracting = input.GetPlayerConfirmPressed();
        if(!isInteracting) return;

        //切换游戏模式
        GameModeManager gameMode = GameModeManager.Instance;
        gameMode.ChangeGameMode();
    }
}
