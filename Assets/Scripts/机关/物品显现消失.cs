using UnityEngine;

public class ToggleObjects2D : MonoBehaviour
{
    [Header("触发后显示的物体")]
    public GameObject[] showObjects;

    [Header("触发后隐藏的物体")]
    public GameObject[] hideObjects;

    [Header("是否只触发一次（机关不还原）")]
    public bool onlyTriggerOnce = false;

    private bool isTriggered = false;

    // 机关激活时调用（踩下压力板）
    public void Activate()
    {
        // 如果只触发一次，已经触发过就不再执行
        if (onlyTriggerOnce && isTriggered) return;

        isTriggered = true;

        // 显示指定物体
        foreach (GameObject obj in showObjects)
        {
            if (obj != null) obj.SetActive(true);
        }

        // 隐藏指定物体
        foreach (GameObject obj in hideObjects)
        {
            if (obj != null) obj.SetActive(false);
        }
    }

    // 机关关闭时调用（离开压力板）
    public void Deactivate()
    {
        // 只触发一次的机关，无法关闭
        if (onlyTriggerOnce) return;

        isTriggered = false;

        // 还原：隐藏之前显示的
        foreach (GameObject obj in showObjects)
        {
            if (obj != null) obj.SetActive(false);
        }

        // 还原：显示之前隐藏的
        foreach (GameObject obj in hideObjects)
        {
            if (obj != null) obj.SetActive(true);
        }
    }
}
