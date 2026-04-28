using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    [SerializeField] private Image coolDownImage;
    [SerializeField] private Image SwordImage;
    [SerializeField] private Image FlaskImage;

    [SerializeField] private SkillManager skills;

    [Header("Souls info")]
    [SerializeField] private TextMeshProUGUI currentSouls;
    [SerializeField] private float soulsAmount;
    [SerializeField] private float increaseRate = 100;

    void Start()
    {
        skills = SkillManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSoulsUI();

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            SetCoolDown(coolDownImage);
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            SetCoolDown(SwordImage);
        }
        if (Input.GetKeyDown(KeyCode.R) && Inventory.instance.GetEquipment(EquipmentType.Flask) != null)
        {
            SetCoolDown(FlaskImage);
        }

        CheckCoolDown(FlaskImage, Inventory.instance.flaskCooldown);
        CheckCoolDown(coolDownImage, skills.dash.cooldown);
        CheckCoolDown(SwordImage, skills.sword.cooldown);
    }

    private void UpdateSoulsUI()
    {
        if (soulsAmount < PlayerManager.Instance.CurrentCurrencyAmount())
        {
            soulsAmount += Time.deltaTime * increaseRate;
        }
        else
        {
            soulsAmount = PlayerManager.Instance.CurrentCurrencyAmount();
        }

        currentSouls.text = ((int)soulsAmount).ToString();
    }

    private void SetCoolDown(Image _image)
    {
        if (_image.fillAmount <= 0)
        {
            _image.fillAmount = 1;
        }
    }

    private void CheckCoolDown(Image _image, float _cooldown)
    {
        if (_image.fillAmount > 0)
        {
            _image.fillAmount -= 1 / _cooldown * Time.deltaTime;
        }
    }
}
