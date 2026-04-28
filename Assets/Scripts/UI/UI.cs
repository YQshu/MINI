using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    [SerializeField] private UI_FadeScreen fadeScreen;
    [SerializeField] private GameObject endText;
    [SerializeField] private GameObject restartButton;
    [Space]

    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject SkillTreeUI;
    [SerializeField] private GameObject CraftUI;
    [SerializeField] private GameObject OptionsUI;
    [SerializeField] private GameObject InGameUI;

    public UI_SkillSlot skillSlotTip;
    public UI_ItemToolTip itemToolTip;
    public UI_StatToolTip statToolTip;
    public UI_CraftWindow craftWindow;


    private void Awake()
    {
        fadeScreen.gameObject.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        SwitchTo(InGameUI);

        itemToolTip.gameObject.SetActive(false);
        statToolTip.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchWithKeyTo(characterUI);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchWithKeyTo(SkillTreeUI);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchWithKeyTo(CraftUI);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchWithKeyTo(OptionsUI);
        }
    }

    public void SwitchTo(GameObject _nemu)
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            bool isFadeScreen = transform.GetChild(i).GetComponent<UI_FadeScreen>() != null;
            if (isFadeScreen == false)
                transform.GetChild(i).gameObject.SetActive(false);
        }

        if (_nemu != null)
        {
            _nemu.SetActive(true);
        }
        if (GameManager.instance != null)
        {
            if (_nemu == InGameUI)
            {
                GameManager.instance.PauseGame(false);
            }
            else
            {
                GameManager.instance.PauseGame(true);
            }
        }
    }

    public void SwitchWithKeyTo(GameObject _menu)
    {
        if (_menu != null && _menu.activeSelf)
        {
            _menu.SetActive(false);
            CheckForInGameUI();
            return;
        }
        SwitchTo(_menu);
    }
    private void CheckForInGameUI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf && transform.GetChild(i).GetComponent<UI_FadeScreen>() == null)
            {
                return;
            }
        }
        SwitchTo(InGameUI);
    }

    public void SwitchOnEndScreen()
    {
        fadeScreen.FadeOut();
        StartCoroutine(EndScreenCorutione());
    }
    IEnumerator EndScreenCorutione()
    {
        yield return new WaitForSeconds(1);
        endText.SetActive(true);
        yield return new WaitForSeconds(1);
        restartButton.SetActive(true);
    }
    public void RestartGameButton() => GameManager.instance.RestartGame();

    public void SaveAndExit()
    {
        Debug.Log("SaveAndExit 被点击了！");

        // 保存游戏
        if (Savemanage.instance != null)
            Savemanage.instance.SaveGame();

        // 播放淡出
        if (fadeScreen != null)
            fadeScreen.FadeOut();

        // 直接退出
        Debug.Log("游戏退出");

        SceneManager.LoadScene("MainMenu");
    }
}
