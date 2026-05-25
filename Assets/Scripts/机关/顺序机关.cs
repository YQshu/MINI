using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class SequentialSwitch2D : MonoBehaviour
{
    [Header("设置")]
    public List<GameObject> switchesInOrder; // 按正确顺序排列的开关列表
    public UnityEvent OnComplete; // 全部按顺序触发成功
    public UnityEvent OnWrongOrder; // 顺序错误时触发（可选）

    [Header("状态")]
    public bool isCompleted = false; // 是否已完成
    public int currentStep = 0;      // 当前进度（第几个）

    private HashSet<GameObject> wrongSwitches = new HashSet<GameObject>(); // 记录错误触发的开关

    // 每个开关触发时调用这个方法
    public void OnSwitchTriggered(GameObject triggeredSwitch)
    {
        // 已经完成了，不再响应
        if (isCompleted) return;

        // 检查是否在正确顺序列表中
        if (currentStep < switchesInOrder.Count && triggeredSwitch == switchesInOrder[currentStep])
        {
            // 顺序正确！
            currentStep++;
            Debug.Log($"✓ 顺序正确！进度: {currentStep}/{switchesInOrder.Count}");

            // 检查是否完成
            if (currentStep >= switchesInOrder.Count)
            {
                CompleteSequence();
            }
        }
        else
        {
            // 顺序错误！
            Debug.Log($"✗ 顺序错误！应该触发 {GetCurrentSwitchName()}，但触发了 {triggeredSwitch.name}");

            // 记录错误触发的开关（用于可选反馈）
            if (!wrongSwitches.Contains(triggeredSwitch))
            {
                wrongSwitches.Add(triggeredSwitch);
            }

            // 触发顺序错误事件
            OnWrongOrder?.Invoke();

            // 可选：重置序列（取消下面这行注释则错误后重置）
            // ResetSequence();
        }
    }

    private string GetCurrentSwitchName()
    {
        if (currentStep < switchesInOrder.Count && switchesInOrder[currentStep] != null)
            return switchesInOrder[currentStep].name;
        return "无";
    }

    private void CompleteSequence()
    {
        isCompleted = true;
        Debug.Log("🎉 所有开关按正确顺序触发！成功！");
        OnComplete?.Invoke();
    }

    // 重置序列（让玩家重新挑战）
    public void ResetSequence()
    {
        currentStep = 0;
        isCompleted = false;
        wrongSwitches.Clear();
        Debug.Log("序列已重置，请按顺序重新触发");
    }

    // 获取当前需要触发的开关（用于UI提示）
    public GameObject GetCurrentRequiredSwitch()
    {
        if (currentStep < switchesInOrder.Count)
            return switchesInOrder[currentStep];
        return null;
    }

    // 获取进度文本（用于UI显示）
    public string GetProgressText()
    {
        return $"{currentStep}/{switchesInOrder.Count}";
    }
}
