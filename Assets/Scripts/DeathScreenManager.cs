using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DeathScreenManager : MonoBehaviour
{
    [Header("Death Screen UI")]
    [SerializeField] private GameObject deathScreenPanel;
    [SerializeField] private Button continueButton;
    [SerializeField] private CanvasGroup deathCanvasGroup;

    [Header("Animation Timing")]
    [SerializeField] private float animationDelay = 0.4f;
    [SerializeField] private float fadeInDuration = 0.8f;
    [SerializeField] private float fadeOutDuration = 0.3f;

    [Header("Player Reference")]
    [SerializeField] private Player player;

    private Coroutine currentRoutine;

    private void Awake()
    {
        if (continueButton != null)
            continueButton.onClick.AddListener(OnContinueClicked);
    }

    private void Start()
    {
        if (deathScreenPanel != null)
            deathScreenPanel.SetActive(false);

        if (deathCanvasGroup != null)
            deathCanvasGroup.alpha = 0f;
    }

    public void ShowDeathScreen()
    {
        if (deathScreenPanel != null)
            deathScreenPanel.SetActive(true);

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(DeathScreenFadeInRoutine());
    }

    private IEnumerator DeathScreenFadeInRoutine()
    {
        // Wait for death animation to finish playing
        yield return new WaitForSecondsRealtime(animationDelay);

        // Pause the game
        Time.timeScale = 0f;

        // Enable cursor for UI interaction
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Fade in the death screen UI
        float elapsed = 0f;
        while (elapsed < fadeInDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            if (deathCanvasGroup != null)
                deathCanvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeInDuration);
            yield return null;
        }

        if (deathCanvasGroup != null)
            deathCanvasGroup.alpha = 1f;
    }

    public void OnContinueClicked()
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(DeathScreenFadeOutAndRespawnRoutine());
    }

    private IEnumerator DeathScreenFadeOutAndRespawnRoutine()
    {
        // Fade out the death screen UI
        float elapsed = 0f;
        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            if (deathCanvasGroup != null)
                deathCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeOutDuration);
            yield return null;
        }

        if (deathCanvasGroup != null)
            deathCanvasGroup.alpha = 0f;

        if (deathScreenPanel != null)
            deathScreenPanel.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Time.timeScale = 1f;

        // Respawn player at last checkpoint position
        if (player != null)
        {
            var saveManager = CheckpointSaveManager.Instance;
            if (saveManager != null && saveManager.HasSavedPosition)
            {
                player.transform.position = saveManager.LastRespawnPosition;
            }

            player.Respawn();
            player.input.Enable();
            player.stateMachine.ChangeState(player.idleState);
        }
    }

    private void OnDestroy()
    {
        if (continueButton != null)
            continueButton.onClick.RemoveListener(OnContinueClicked);
    }
}