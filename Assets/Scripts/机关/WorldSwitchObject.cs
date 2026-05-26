using UnityEngine;

public class WorldSwitchObject : MonoBehaviour
{
    public KeyCode switchKey = KeyCode.LeftShift;
    public bool startVisible = true;

    private bool isVisible;

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
        var renderers = GetComponentsInChildren<Renderer>(true);
        var colliders = GetComponentsInChildren<Collider2D>(true);

        foreach (var r in renderers)
            r.enabled = isVisible;

        foreach (var c in colliders)
            c.enabled = isVisible;
    }
}