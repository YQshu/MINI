using UnityEngine;
using System.Collections;

public class SimpleWorldSwitchManager : MonoBehaviour
{
    // 单例模式，方便其他脚本访问
    public static SimpleWorldSwitchManager Instance;

    // 定义一个枚举，清晰表示世界状态
    public enum World { Light, Shadow }
    public World CurrentWorld { get; private set; } = World.Light; // 默认初始世界为光界

    // 在Inspector面板中拖拽赋值
    [Header("世界图层")]
    public GameObject lightWorldParent;   // 拖入 World_Light
    public GameObject shadowWorldParent;  // 拖入 World_Shadow

    [Header("切换设置")]
    public KeyCode switchKey = KeyCode.LeftShift; // 切换按键
    public float switchCooldown = 0.1f;            // 防止按键连按的极短冷却
    public float transitionFadeTime = 0.15f;       // 切换时的短暂黑屏/白屏时间

    // 用于管理切换状态
    private bool isSwitching = false;
    private float lastSwitchTime = 0f;

    void Awake()
    {
        // 初始化单例
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // 游戏开始时，确保只有光界是激活的
        InitializeWorlds();
    }

    void Update()
    {
        // 检测切换按键，并满足冷却条件
        if (Input.GetKeyDown(switchKey) && !isSwitching && Time.time > lastSwitchTime + switchCooldown)
        {
            StartCoroutine(SwitchWorldRoutine());
        }
    }

    /// <summary>
    /// 初始化世界状态
    /// </summary>
    private void InitializeWorlds()
    {
        if (lightWorldParent != null) lightWorldParent.SetActive(true);
        if (shadowWorldParent != null) shadowWorldParent.SetActive(false);
        CurrentWorld = World.Light;
        Debug.Log("世界初始化：光界");
    }

    /// <summary>
    /// 执行世界切换的协程
    /// </summary>
    private IEnumerator SwitchWorldRoutine()
    {
        isSwitching = true;
        lastSwitchTime = Time.time;

        // 1. 可选：在这里触发一个简单的屏幕闪白/闪黑效果
        // 例如：UIManager.Instance.FadeScreen(0.1f);
        Debug.Log("开始世界切换...");

        // 模拟一个短暂的转场（策划案中提到的0.2秒）
        yield return new WaitForSeconds(transitionFadeTime / 2);

        // 2. 执行核心的激活/禁用逻辑
        PerformWorldSwitch();

        yield return new WaitForSeconds(transitionFadeTime / 2);

        // 3. 切换完成
        Debug.Log($"世界切换完成。当前世界：{CurrentWorld}");
        isSwitching = false;
    }

    /// <summary>
    /// 实际执行世界图层切换的核心方法
    /// </summary>
    private void PerformWorldSwitch()
    {
        // 根据当前世界决定目标世界
        World targetWorld = (CurrentWorld == World.Light) ? World.Shadow : World.Light;

        // 切换两个父对象的激活状态
        if (lightWorldParent != null)
            lightWorldParent.SetActive(targetWorld == World.Light);
        if (shadowWorldParent != null)
            shadowWorldParent.SetActive(targetWorld == World.Shadow);

        // 更新当前世界状态
        CurrentWorld = targetWorld;

        // 可以在这里触发一个事件，通知游戏内其他系统（如声音、特效）世界已改变
        // OnWorldSwitched?.Invoke(CurrentWorld);
    }
}