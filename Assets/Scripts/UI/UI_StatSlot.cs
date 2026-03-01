using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_StatSlot : MonoBehaviour , IPointerEnterHandler , IPointerExitHandler
{
    private UI ui;
    [SerializeField] private string statName;
    [SerializeField] private StatType statType;
    [SerializeField] private TextMeshProUGUI statValueText;
    [SerializeField] private TextMeshProUGUI statNameText;

    [TextArea]
    [SerializeField] private string statDescription;

    private void OnValidate()
    {
        gameObject.name = "Stat - " + statName;

        if (statValueText != null)
        {
            statValueText.text = statName;
        }
    }
    void Start()
    {
        UpdateStatValue();
        ui = GetComponentInParent<UI>();
    }

    public void UpdateStatValue()
    {
        PlayerStats playerStats = PlayerManager.Instance.player.GetComponent<PlayerStats>();

        if (playerStats != null)
        {
            statValueText.text = playerStats.GetStat(statType).getValue().ToString();

            if(statType == StatType.health)
            {
                statValueText.text = playerStats.GetMaxHp().ToString();
            }
            if(statType == StatType.damage)
            {
                statValueText.text = (playerStats.damage.getValue() + playerStats.GetStat(StatType.strength).getValue()).ToString();
            }
            if(statType == StatType.cirtPower)
            {
                statValueText.text = (playerStats.critPower.getValue() + playerStats.GetStat(StatType.strength).getValue()).ToString();
            }
            if(statType == StatType.cirtChance)
            {
                statValueText.text = (playerStats.critChance.getValue() + playerStats.GetStat(StatType.agllity).getValue()).ToString();
            }
            if(statType == StatType.evasion)
            {
                statValueText.text = (playerStats.evasion.getValue() + playerStats.GetStat(StatType.agllity).getValue()).ToString();
            }
            if(statType == StatType.magicRes)
            {
                statValueText.text = (playerStats.magicResitance.getValue() + playerStats.GetStat(StatType.intelligence).getValue() * 3).ToString();
            }

        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.statToolTip.ShowStatToolTip(statDescription);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.statToolTip.HideStatToolTip();
    }
}
