using UnityEngine;

public class LockingPressurePlate2D : MonoBehaviour
{
    [Header("设置")]
    public string boxTag = "PushableBox";  // 石箱的Tag
    public BoolEvent OnPlateActivated;     // 激活事件（只触发一次）

    [Header("状态")]
    public bool isActivated = false;       // 是否已永久激活
    public bool isLocked = false;          // 是否已锁定箱子

    private GameObject lockedBox;          // 被锁定的箱子

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 已经激活过了，不再响应
        if (isActivated) return;

        // 必须是石箱才能触发
        if (other.CompareTag(boxTag))
        {
            ActivateAndLock(other.gameObject);
        }
    }

    private void ActivateAndLock(GameObject box)
    {
        isActivated = true;
        lockedBox = box;

        // 触发事件（开门、开机关等）
        OnPlateActivated?.Invoke(true);

        // 锁定箱子，使其不能再被推动
        LockBox(box);

        Debug.Log($"压力板已永久激活，箱子 {box.name} 被锁定");
    }

    private void LockBox(GameObject box)
    {
        isLocked = true;

        // 获取箱子的推动脚本并禁用
        PushableBox pushable = box.GetComponent<PushableBox>();
        if (pushable != null)
        {
            pushable.enabled = false;
        }

        // 锁定刚体，防止移动
        Rigidbody2D rb = box.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;  // 完全冻结
            rb.velocity = Vector2.zero;  // 清除速度
        }

        // 可选：改变箱子颜色表示已锁定
        SpriteRenderer sr = box.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = Color.gray;  // 变成灰色表示已固定
        }
    }

    // 调试信息：在Scene视图中显示状态
    private void OnDrawGizmos()
    {
        if (isActivated)
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.red;

        Gizmos.DrawWireCube(transform.position, GetComponent<Collider2D>().bounds.size);
    }
}
