using UnityEngine;
using UnityEngine.Events;

public class StepShadowSwitch2D : MonoBehaviour
{
    public string playerTag = "Interactable";
    public UnityEvent OnSwitchTriggered;
    private bool isTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isTriggered || !other.CompareTag(playerTag)) return;

        isTriggered = true;
        OnSwitchTriggered?.Invoke();
        GetComponent<Collider2D>().enabled = false;
    }
}