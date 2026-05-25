using UnityEngine;
using System.Collections.Generic;

public class SwitchResetter : MonoBehaviour
{
    // 存储每个开关的原始状态
    private Dictionary<GameObject, SwitchState> switchStates = new Dictionary<GameObject, SwitchState>();

    // 开关状态数据
    private class SwitchState
    {
        public bool wasActivated;
        public Vector3 originalRotation;
        public ManualSwitch2D switchComponent;
    }

    // 注册一个开关，保存它的原始状态
    public void RegisterSwitch(GameObject switchObj)
    {
        if (switchStates.ContainsKey(switchObj)) return;

        ManualSwitch2D manualSwitch = switchObj.GetComponent<ManualSwitch2D>();
        if (manualSwitch != null)
        {
            SwitchState state = new SwitchState();
            state.switchComponent = manualSwitch;
            state.originalRotation = switchObj.transform.eulerAngles;
            state.wasActivated = false;

            switchStates[switchObj] = state;
        }
    }

    // 重置所有已注册的开关
    public void ResetAllSwitches()
    {
        foreach (var kvp in switchStates)
        {
            ResetSwitch(kvp.Key, kvp.Value);
        }
        Debug.Log($"已重置 {switchStates.Count} 个开关");
    }

    // 重置单个开关
    private void ResetSwitch(GameObject switchObj, SwitchState state)
    {
        if (switchObj == null) return;

        // 使用反射重置私有字段 isActivated
        var isActivatedField = typeof(ManualSwitch2D).GetField("isActivated",
            System.Reflection.BindingFlags.NonPublic |
            System.Reflection.BindingFlags.Instance);

        if (isActivatedField != null)
        {
            isActivatedField.SetValue(state.switchComponent, false);
        }

        // 恢复旋转
        switchObj.transform.eulerAngles = state.originalRotation;

        Debug.Log($"已重置开关: {switchObj.name}");
    }

    // 重置指定开关（通过GameObject）
    public void ResetSwitchByName(GameObject switchObj)
    {
        if (switchStates.ContainsKey(switchObj))
        {
            ResetSwitch(switchObj, switchStates[switchObj]);
        }
    }
}