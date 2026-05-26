using UnityEngine;

public class Trap : MonoBehaviour
{
    [Header("Trap details")]
    public int damage = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();
        if (player != null)
        {
            player.TakeDamage(damage);
        }
    }
}