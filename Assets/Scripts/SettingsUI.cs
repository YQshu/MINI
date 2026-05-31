using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SettingsUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private Button backButton;
    [SerializeField] private Button deleteSaveButton;
    [SerializeField] private Button quitGameButton;

    [Header("Player Reference")]
    [SerializeField] private Player player;

    private bool isOpen;

    private void Awake()
    {
        if (backButton != null)
            backButton.onClick.AddListener(OnBackClicked);
        if (deleteSaveButton != null)
            deleteSaveButton.onClick.AddListener(OnDeleteSaveClicked);
        if (quitGameButton != null)
            quitGameButton.onClick.AddListener(OnQuitGameClicked);
    }

    private void Start()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    private void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            if (isOpen)
                CloseSettings();
            else
                OpenSettings();
        }
    }

    private void OpenSettings()
    {
        isOpen = true;

        if (settingsPanel != null)
            settingsPanel.SetActive(true);

        Time.timeScale = 0f;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (player != null)
            player.input.Disable();
    }

    private void CloseSettings()
    {
        isOpen = false;

        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Time.timeScale = 1f;

        if (player != null)
            player.input.Enable();
    }

    private void OnBackClicked()
    {
        CloseSettings();
    }

    private void OnDeleteSaveClicked()
    {
        if (CheckpointSaveManager.Instance != null)
            CheckpointSaveManager.Instance.ClearSave();
    }

    private void OnQuitGameClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void OnDestroy()
    {
        if (backButton != null)
            backButton.onClick.RemoveListener(OnBackClicked);
        if (deleteSaveButton != null)
            deleteSaveButton.onClick.RemoveListener(OnDeleteSaveClicked);
        if (quitGameButton != null)
            quitGameButton.onClick.RemoveListener(OnQuitGameClicked);
    }
}
