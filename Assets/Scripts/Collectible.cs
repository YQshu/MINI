using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Collect Settings")]
    public KeyCode interactKey = KeyCode.E;
    public string playerTag = "Interactable";

    private bool playerInRange;

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            var player = FindObjectOfType<Player>();
            if (player != null)
            {
                player.AddCoin();
                player.stateMachine.ChangeState(player.collectState);

                GetComponent<Collider2D>().enabled = false;
                GetComponent<SpriteRenderer>().enabled = false;

                Destroy(gameObject, player.collectState.collectDuration + 0.1f);
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
}
