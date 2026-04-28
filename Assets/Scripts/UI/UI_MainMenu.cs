using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private string sceneName = "MainScene";
    [SerializeField] private GameObject continueButton;
    [SerializeField] UI_FadeScreen fadeScreen;
    [SerializeField] private float fadeDuration = 1.5f;
    private void Start()
    {
        if (Savemanage.instance.HasSaveData() == false)
        {
            continueButton.SetActive(false);
        }
    }

    public void ContinueGame()
    {
        StartCoroutine(LoadSceneWithFade(1.5f));
    }

    public void NewGame()
    {
        StartCoroutine(LoadSceneWithFade(1.5f));
        Savemanage.instance.DeleteSavedData();
    }

    public void ExitGame()
    {
        StartCoroutine(ExitWithFade());
    }
    IEnumerator LoadSceneWithFade(float _delay)
    {
        fadeScreen.FadeOut();

        yield return new WaitForSeconds(_delay);
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator ExitWithFade()
    {
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(fadeDuration);

        Debug.Log("ÓÎÏ·ÍË³ö");

#if UNITY_EDITOR
        // ±à¼­Æ÷Ä£Ê½£ºÍ£Ö¹²¥·Å
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}

