using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button startButton;
    [SerializeField] private Text subtitleText;

    [Header("Scene")]
    [SerializeField] private string gameSceneName = "MainScene";

    [Header("Animation")]
    [SerializeField] private float fadeOutDuration = 1f;

    private CanvasGroup canvasGroup;
    private bool isTransitioning;

    private void Awake()
    {
        if (startButton != null)
            startButton.onClick.AddListener(OnStartClicked);

        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        if (isTransitioning) return;

        // Also allow any key to start
        if (Input.anyKeyDown)
        {
            OnStartClicked();
        }

        // Subtitle blink effect
        if (subtitleText != null)
        {
            float alpha = Mathf.PingPong(Time.time * 1.5f, 1f);
            subtitleText.color = new Color(subtitleText.color.r, subtitleText.color.g, subtitleText.color.b, alpha);
        }
    }

    private void OnStartClicked()
    {
        if (isTransitioning) return;
        isTransitioning = true;

        StartCoroutine(FadeOutAndLoadScene());
    }

    private IEnumerator FadeOutAndLoadScene()
    {
        if (canvasGroup != null)
        {
            float elapsed = 0f;
            while (elapsed < fadeOutDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeOutDuration);
                yield return null;
            }
            canvasGroup.alpha = 0f;
        }

        SceneManager.LoadScene(gameSceneName);
    }

    private void OnDestroy()
    {
        if (startButton != null)
            startButton.onClick.RemoveListener(OnStartClicked);
    }
}
