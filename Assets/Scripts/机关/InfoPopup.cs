using UnityEngine;
using TMPro;

public class InfoPopup : MonoBehaviour
{
    [Header("Popup Settings")]
    public string popupText = "按E互动";
    public string playerTag = "Interactable";
    public float popupHeight = 1.2f;
    public int fontSize = 24;
    public Color textColor = Color.yellow;
    public Color outlineColor = Color.black;
    public float outlineWidth = 3f;

    private GameObject popupGo;
    private TextMeshPro textMesh;
    private bool dismissed;

    private void Start()
    {
        CreatePopup();
        HidePopup();
    }

    private void CreatePopup()
    {
        popupGo = new GameObject("Popup");
        popupGo.transform.SetParent(transform);
        popupGo.transform.localPosition = new Vector3(0, popupHeight, -1);

        textMesh = popupGo.AddComponent<TextMeshPro>();
        textMesh.text = popupText;
        textMesh.fontSize = fontSize;
        textMesh.color = textColor;
        textMesh.alignment = TextAlignmentOptions.Center;

        // Built-in outline
        textMesh.outlineColor = outlineColor;
        textMesh.outlineWidth = outlineWidth;
        textMesh.fontSharedMaterial.EnableKeyword("OUTLINE_ON");

        var renderer = popupGo.GetComponent<MeshRenderer>();
        renderer.sortingLayerName = "Item";
        renderer.sortingOrder = 9999;

        popupGo.AddComponent<Billboard>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (dismissed) return;
        if (other.CompareTag(playerTag))
            ShowPopup();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (dismissed) return;
        if (other.CompareTag(playerTag))
            HidePopup();
    }

    public void Dismiss()
    {
        dismissed = true;
        HidePopup();
    }

    private void ShowPopup()
    {
        if (popupGo != null) popupGo.SetActive(true);
    }

    private void HidePopup()
    {
        if (popupGo != null) popupGo.SetActive(false);
    }

    private void OnDestroy()
    {
        if (popupGo != null) Destroy(popupGo);
    }
}

public class Billboard : MonoBehaviour
{
    private void LateUpdate()
    {
        if (Camera.main != null)
            transform.rotation = Camera.main.transform.rotation;
    }
}
