namespace MyFramework.Event;

/// <summary>
/// 表示一个事件接收者接口，用于接收并处理指定类型的事件。
/// </summary>
public interface IEventReceiver<TEvent> where TEvent : IEvent
{
    // 处理接收到的事件实例。
    void OnEvent(TEvent evt);
}
