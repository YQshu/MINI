using UnityEngine;
using TMPro;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private TextMeshProUGUI healthText;

    private void Start()
    {
        if (healthText == null)
            healthText = GetComponent<TextMeshProUGUI>();

        if (player == null)
            player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        if (player == null || healthText == null) return;

        int current = player.currentHealth;
        int max = player.maxHealth;

        healthText.text = $"HP: {current} / {max}";

        // Color feedback: red when low health
        if (current <= max * 0.3f)
            healthText.color = Color.red;
        else if (current <= max * 0.6f)
            healthText.color = new Color(1f, 0.65f, 0f); // orange
        else
            healthText.color = Color.white;
    }
}