//游戏模式管理器
public class GameModeManager : Singleton<GameModeManager>
{
    public GameMode currentGameMode;

    [SerializeField] private GameMode defaultMode = GameMode.Light;

    protected override void Awake()
    {
        base.Awake();
        currentGameMode = defaultMode;
    }

    
    private void Start()
    {
        //广播当前游戏模式    
        EventBus.Publish(new GameModeChangeEvent(currentGameMode));
    }

    //切换游戏模式
    public void ChangeGameMode()
    {
        if (currentGameMode == GameMode.Shadow)
        {
            currentGameMode = GameMode.Light;
        }
        else
        {
            currentGameMode = GameMode.Shadow;
        }
        EventBus.Publish(new GameModeChangeEvent(currentGameMode));
    }
}
