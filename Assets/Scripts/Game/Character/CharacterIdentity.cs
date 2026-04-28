/// <summary>
/// 获取人物信息
/// </summary>
public class CharacterIdentity : MonoBehaviour
{
    [SerializeField] private CharacterDifinitionSO _characterDifinition;

    public CharacterDifinitionSO GetCharacterDifinition() => _characterDifinition;

    //快速设定人物属性
    public void SetDefinition(CharacterDifinitionSO characterDifinition)
    {
        _characterDifinition = characterDifinition;
    }
}
