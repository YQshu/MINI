using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Checkpoint Settings")]
    public int checkpointId = 0;
    public string playerTag = "Interactable";

    private bool isActivated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isActivated || !other.CompareTag(playerTag)) return;

        Activate();
    }

    private void Activate()
    {
        isActivated = true;

        CheckpointSaveManager.Instance?.ActivateCheckpoint(checkpointId, transform.position);

        // Visual feedback: change color to green
        var sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.color = Color.green;

        Debug.Log($"Checkpoint {checkpointId} activated at {transform.position}");
    }
}