using UnityEngine;

public class Door2D : MonoBehaviour
{
    [Header("设置")]
    public bool isOpen = false;
    public Vector2 openOffset = new Vector2(0, 3f); // 门打开时的位移
    public float moveSpeed = 2f;

    private Vector2 closedPos, openPos;

    void Awake()
    {
        closedPos = transform.position;
        openPos = closedPos + openOffset;
    }

    // 给外部机关调用的控制方法
    public void ToggleDoor() => isOpen = !isOpen;
    public void OpenDoor() => isOpen = true;
    public void CloseDoor() => isOpen = false;

    void Update()
    {
        Vector2 targetPos = isOpen ? openPos : closedPos;
        transform.position = Vector2.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
    }
}