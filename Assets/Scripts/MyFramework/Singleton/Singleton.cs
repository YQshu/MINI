/// <summary>
/// Unity  MonoBehaviour 泛型单例基类
/// 确保场景中指定类型 <typeparamref name="T"/> 的组件实例唯一
/// </summary>
/// <typeparam name="T">单例类型，必须继承自 <see cref="MonoBehaviour"/></typeparam>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    // 私有静态单例实例
    private static T _instance;

    //公共静态访问器，获取单例实例
    public static T Instance => _instance;

    //初始化单例逻辑：
    //1. 若实例为空，则将当前组件赋值给实例
    //2. 若实例已存在且不是当前组件，则销毁当前游戏对象，确保单例唯一性
    protected virtual void Awake()
    {
        if (_instance is null)
        {
            _instance = this as T;
        }

        if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
}