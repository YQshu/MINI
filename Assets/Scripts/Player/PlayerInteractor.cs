/// <summary>
/// 角色进入物体碰撞处理 -- 待开发
/// </summary>
public class PlayerInteractor : MonoBehaviour
{
    private CharacterIdentity _characterIdentity;

    private void Awake()
    {
        _characterIdentity = GetComponent<CharacterIdentity>(); 
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent(out InteractionBase interactionBase))
        {
            interactionBase.OnFocus(_characterIdentity.GetCharacterDifinition());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out InteractionBase interactionBase))
        {
            interactionBase.OnLoseFocus(_characterIdentity.GetCharacterDifinition());
        }
    }
}
