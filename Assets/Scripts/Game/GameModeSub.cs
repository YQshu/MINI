using MyFramework.Event;

/// <summary>
/// 物体订阅游戏模式改变而改变--测试
/// </summary>
public class GameModeSub : MonoBehaviour, IEventReceiver<GameModeChangeEvent>
{
    [SerializeField]private GameObject subObject;

    //注册事件
    private void OnEnable()
    {
        EventBus.Subscribe<GameModeChangeEvent>(this);
    }

    //取消事件
    private void OnDisable()
    {
        EventBus.Unsubscribe<GameModeChangeEvent>(this);
    }

    //实现接口
    public void OnEvent(GameModeChangeEvent evt)
    {
        switch (evt.newMode)
        {
            case GameMode.Shadow:
                SetActive(false);
                break;
            case GameMode.Light:
                SetActive(true);
                break;
        }
    }

    private void SetActive(bool isActive)
    {
        subObject.SetActive(isActive);
    }
}
