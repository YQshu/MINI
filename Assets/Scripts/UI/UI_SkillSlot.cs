using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_SkillSlot : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI skillText;
    [SerializeField] protected TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillCost;
    [SerializeField] private float defaultSize;

    public void ShowToolTip(string _skillDescription , string _skillName, int _price)
    {
        skillName.text = _skillName;
        skillText.text = _skillDescription;
        skillCost.text = "Cost: "+_price;

        AdjustPosition();
        AdjustFontSize(skillName);

        gameObject.SetActive(true);
    }
    public void HideToolTip()
    {
        skillName.fontSize = defaultSize;
        gameObject.SetActive(false);
    }
}
