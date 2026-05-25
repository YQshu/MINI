using UnityEngine;

public class ContinuousPressurePlate2D : MonoBehaviour
{
    [Header("设置")]
    public string[] validTags = { "Interactable", "PushableBox" };  // Interactable是玩家标签，多个有效标签
    public Door2D targetDoor;

    private int triggerCount = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsValidTag(other.tag))
        {
            triggerCount++;
            if (triggerCount == 1)
                targetDoor?.OpenDoor();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (IsValidTag(other.tag))
        {
            triggerCount--;
            if (triggerCount == 0)
                targetDoor?.CloseDoor();
        }
    }

    private bool IsValidTag(string tag)
    {
        foreach (string validTag in validTags)
        {
            if (tag == validTag)
                return true;
        }
        return false;
    }
}