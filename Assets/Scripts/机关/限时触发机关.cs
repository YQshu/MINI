using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class TimedSequenceSwitch2D : MonoBehaviour
{
    [Header("设置")]
    public List<GameObject> switches; // 要触发的开关列表
    public float timeLimit = 10f;
    public UnityEvent OnComplete; // 全部触发成功
    public UnityEvent OnFailed;   // 超时失败

    [Header("复位设置")]
    public SwitchResetter switchResetter; // 拖入复位器组件

    private HashSet<GameObject> triggeredSwitches = new HashSet<GameObject>();
    private float timer;
    private bool isRunning = false;
    private bool isCompleted = false;

    void Start()
    {
        // 如果没有指定复位器，自动添加
        if (switchResetter == null)
        {
            switchResetter = GetComponent<SwitchResetter>();
            if (switchResetter == null)
            {
                switchResetter = gameObject.AddComponent<SwitchResetter>();
            }
        }

        // 注册所有开关
        foreach (GameObject switchObj in switches)
        {
            switchResetter.RegisterSwitch(switchObj);
        }
    }

    public void OnSwitchTriggered(GameObject triggeredSwitch)
    {
        if (isCompleted) return;

        if (!isRunning && !isCompleted)
        {
            StartSequence();
        }

        if (!isRunning) return;

        if (switches.Contains(triggeredSwitch))
        {
            if (!triggeredSwitches.Contains(triggeredSwitch))
            {
                triggeredSwitches.Add(triggeredSwitch);
                Debug.Log($"触发开关 {triggeredSwitch.name}，已完成 {triggeredSwitches.Count}/{switches.Count}");
            }
            else
            {
                Debug.Log($"开关 {triggeredSwitch.name} 已经触发过了，无需重复");
            }

            if (triggeredSwitches.Count >= switches.Count)
            {
                CompleteSequence();
            }
        }
        else
        {
            Debug.Log($"开关 {triggeredSwitch.name} 不在序列列表中，忽略");
        }
    }

    private void StartSequence()
    {
        isRunning = true;
        timer = timeLimit;
        triggeredSwitches.Clear();
        Debug.Log($"序列开始！需要在 {timeLimit} 秒内触发所有 {switches.Count} 个开关");
    }

    private void CompleteSequence()
    {
        isRunning = false;
        isCompleted = true;
        Debug.Log("所有开关已触发！成功！");
        OnComplete?.Invoke();
    }

    private void FailSequence()
    {
        isRunning = false;
        Debug.Log($"超时！只触发了 {triggeredSwitches.Count}/{switches.Count} 个开关");
        OnFailed?.Invoke();

        // 使用复位器重置所有开关
        if (switchResetter != null)
        {
            switchResetter.ResetAllSwitches();
        }

        ResetSequence();
    }

    private void ResetSequence()
    {
        triggeredSwitches.Clear();
        isCompleted = false;
    }

    void Update()
    {
        if (!isRunning) return;

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            FailSequence();
        }
    }

    public void ManualReset()
    {
        if (switchResetter != null)
        {
            switchResetter.ResetAllSwitches();
        }
        ResetSequence();
        Debug.Log("序列已手动重置");
    }
}