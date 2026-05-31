using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Checkpoint Settings")]
    public int checkpointId = 0;
    public string playerTag = "Interactable";

    private bool isActivated = false;

    private void Start()
    {
        if (CheckpointSaveManager.Instance != null && CheckpointSaveManager.Instance.IsActivated(checkpointId))
        {
            isActivated = true;
            var sr = GetComponent<SpriteRenderer>();
            if (sr != null)
                sr.color = Color.green;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isActivated || !other.CompareTag(playerTag)) return;

        Activate();
    }

    private void Activate()
    {
        isActivated = true;

        CheckpointSaveManager.Instance?.ActivateCheckpoint(checkpointId, transform.position);

        var sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.color = Color.green;

        Debug.Log($"Checkpoint {checkpointId} activated at {transform.position}");
    }
}