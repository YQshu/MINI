using UnityEngine;
using System.Collections;

public class WorldSwitchObject : MonoBehaviour
{
    public KeyCode switchKey = KeyCode.LeftShift;
    public bool startVisible = true;
    public float fadeDuration = 0.3f;

    private bool isVisible;
    private bool isTransitioning;
    private SpriteRenderer sr;
    private Collider2D col;
    private Coroutine fadeRoutine;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    private void Start()
    {
        isVisible = startVisible;
        if (sr != null)
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, startVisible ? 1f : 0f);
        if (!startVisible && col != null)
            col.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(switchKey) && !isTransitioning)
        {
            isVisible = !isVisible;
            if (fadeRoutine != null)
                StopCoroutine(fadeRoutine);
            fadeRoutine = StartCoroutine(FadeRoutine(isVisible));
        }
    }

    private IEnumerator FadeRoutine(bool show)
    {
        isTransitioning = true;

        if (show && col != null)
            col.enabled = true;

        float startAlpha = sr != null ? sr.color.a : 0f;
        float targetAlpha = show ? 1f : 0f;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;
            if (sr != null)
                sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, Mathf.Lerp(startAlpha, targetAlpha, t));
            yield return null;
        }

        if (sr != null)
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, targetAlpha);

        if (!show && col != null)
            col.enabled = false;

        isTransitioning = false;
    }
}