using UnityEngine;
using UnityEngine.Events;

public class ManualSwitch2D : MonoBehaviour
{
    [Header("设置")]
    public KeyCode interactKey = KeyCode.E;
    public string playerTag = "Interactable";
    public UnityEvent OnSwitchActivated;

    private bool isActivated = false;
    private bool playerInRange = false;

    private void Update()
    {
        // 玩家在范围内、开关未激活时，按E触发
        if (playerInRange && !isActivated && Input.GetKeyDown(interactKey))
        {
            isActivated = true;
            OnSwitchActivated?.Invoke();

            var popup = GetComponent<InfoPopup>();
            if (popup != null) popup.Dismiss();

            // 可选：添加开关拨动动画
            transform.Rotate(0, 0, 90f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag)) playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag)) playerInRange = false;
    }
}