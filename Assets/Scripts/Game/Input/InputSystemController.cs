using MyFramework.Event;
using UnityEngine;

/// <summary>
/// 输入控制
/// </summary>
public class InputSystemController : Singleton<InputSystemController>, IEventReceiver<GameModeChangeEvent>
{
    private CharacterInputAction _inputActions;

    public CharacterInputAction InputActions => _inputActions;

    private bool _isInitialized = false;

    protected override void Awake()
    {
        base.Awake();

        if (!_isInitialized)
        {
            _inputActions ??= new CharacterInputAction();
            _isInitialized = true;
        }
    }

    //获取输入值
    public Vector2 GetMovementInput()
    {
        if (!_isInitialized)
            return Vector2.zero;

        return InputActions.Player.Move.ReadValue<Vector2>();
    }

    //获取确认按键输入
    public bool GetPlayerConfirmPressed()
    {
        if (!_isInitialized)
            return false;

        return _inputActions.Player.Confirm.WasPressedThisFrame();
    }

    //新增：获取跳跃按键输入（按下瞬间）
    public bool GetPlayerJumpPressed()
    {
        if (!_isInitialized)
            return false;

        // 注意：需要在CharacterInputAction中添加Jump动作
        return _inputActions.Player.Jump.WasPressedThisFrame();
    }

    //新增：获取跳跃按键长按状态（用于可变跳跃高度）
    public bool GetPlayerJumpHolding()
    {
        if (!_isInitialized)
            return false;

        return _inputActions.Player.Jump.IsPressed();
    }

    //新增：获取跳跃按键松开瞬间（可选）
    public bool GetPlayerJumpReleased()
    {
        if (!_isInitialized)
            return false;

        return _inputActions.Player.Jump.WasReleasedThisFrame();
    }

    #region 事件实现
    void OnEnable()
    {
        //启用输入系统
        _inputActions.Enable();
        //订阅事件
        EventBus.Subscribe<GameModeChangeEvent>(this);
    }

    private void OnDisable()
    {
        //取消订阅
        EventBus.Unsubscribe<GameModeChangeEvent>(this);
    }

    private void OnDestroy()
    {
        //销毁输入系统资源
        _inputActions.Dispose();
    }

    //预留的玩家操作处理接口实现，便于后面根据游戏模式实现其他功能
    public void OnEvent(GameModeChangeEvent evt)
    {
        //Debug.Log(evt.newMode.ToString());
    }
    #endregion
}