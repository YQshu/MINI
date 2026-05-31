using UnityEngine;
using TMPro;

public class CoinCounterUI : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private TextMeshProUGUI coinText;

    private void Start()
    {
        if (coinText == null)
            coinText = GetComponent<TextMeshProUGUI>();

        if (player == null)
            player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        if (player == null || coinText == null) return;

        int collected = player.collectedCoins;
        int total = player.totalCoins;

        // Show fraction: 3/6
        coinText.text = $"金币: {collected} / {total}";

        // Color feedback: gold when all collected
        if (collected >= total)
            coinText.color = new Color(1f, 0.84f, 0f); // gold
        else if (collected > 0)
            coinText.color = new Color(1f, 1f, 0.6f); // light yellow
        else
            coinText.color = new Color(0.5f, 0.5f, 0.5f); // gray (none yet)
    }
}