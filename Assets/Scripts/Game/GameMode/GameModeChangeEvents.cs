using MyFramework.Event;

//游戏模式改变事件接口
public readonly struct GameModeChangeEvent : IEvent
{
    public readonly GameMode newMode;

    public GameModeChangeEvent(GameMode newMode)
    {
        this.newMode = newMode;
    }
}