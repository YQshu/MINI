using UnityEngine;

public class ContinuousPressurePlate2D : MonoBehaviour
{
    [Header("设置")]
    public string triggerTag = "Interactable"; // 玩家和石箱都设为这个Tag
    public BoolEvent OnPlateStateChanged;

    private int triggerCount = 0; // 记录压在板上的物体数量

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(triggerTag))
        {
            triggerCount++;
            // 第一次被压中时，触发激活事件
            if (triggerCount == 1) OnPlateStateChanged?.Invoke(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(triggerTag))
        {
            triggerCount--;
            // 所有物体离开后，触发复位事件
            if (triggerCount == 0) OnPlateStateChanged?.Invoke(false);
        }
    }
}