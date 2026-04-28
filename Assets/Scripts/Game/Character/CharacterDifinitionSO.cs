/// <summary>
/// 人物属性类
/// </summary>
[CreateAssetMenu(menuName ="Character/Player")]
public class CharacterDifinitionSO : ScriptableObject
{
    [Header("Identity")]
    public string ID;
    public string Name;
    public Sprite Portrait; //人物立绘
}
