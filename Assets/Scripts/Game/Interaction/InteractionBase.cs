/// <summary>
/// 玩家于物体交互基类
/// </summary>
public class InteractionBase : MonoBehaviour
{
    //按交互键时调用
    public void Interact(CharacterDifinitionSO interactor) 
    {
        
    
    }

    //玩家进入
    public void OnFocus(CharacterDifinitionSO interactor) 
    {
        Debug.Log(interactor.name);
    }

    //玩家离开
    public void OnLoseFocus(CharacterDifinitionSO interactor)
    {
        Debug.Log(interactor.ID);
    }

}
