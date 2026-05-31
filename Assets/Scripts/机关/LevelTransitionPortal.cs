using UnityEngine;

public class LevelTransitionPortal : MonoBehaviour
{
    [Header("Portal Settings")]
    [SerializeField] private Vector2 targetPosition;
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private string playerTag = "Interactable";

    [Header("Coin Requirement")]
    [SerializeField] private int requiredCoins = 6;

    private bool playerInRange;

    private void Update()
    {
        if (!playerInRange) return;

        if (Input.GetKeyDown(interactKey))
        {
            var player = FindObjectOfType<Player>();
            if (player == null) return;

            if (player.collectedCoins >= requiredCoins)
            {
                player.portalTargetPosition = targetPosition;
                player.collectedCoins = 0;
                player.stateMachine.ChangeState(player.interactState);

                var popup = GetComponent<InfoPopup>();
                if (popup != null) popup.Dismiss();

                var col = GetComponent<Collider2D>();
                if (col != null) col.enabled = false;
            }
            else
            {
                Debug.Log($"Need {requiredCoins - player.collectedCoins} more coins!");
            }
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(targetPosition, 0.5f);
        Gizmos.DrawLine(transform.position, targetPosition);
    }
}