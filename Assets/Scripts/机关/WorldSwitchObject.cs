using UnityEngine;

public class WorldSwitchObject : MonoBehaviour
{
    public KeyCode switchKey = KeyCode.LeftShift;
    public bool startVisible = true;

    private bool isVisible;
    private SpriteRenderer sr;
    private Collider2D col;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    private void Start()
    {
        isVisible = startVisible;
        ApplyVisibility();
    }

    private void Update()
    {
        if (Input.GetKeyDown(switchKey))
        {
            isVisible = !isVisible;
            ApplyVisibility();
        }
    }

    private void ApplyVisibility()
    {
        if (sr != null) sr.enabled = isVisible;
        if (col != null) col.enabled = isVisible;
    }
}