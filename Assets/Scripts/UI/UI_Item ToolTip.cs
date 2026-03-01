using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ItemToolTip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI ItemNameText;
    [SerializeField] private TextMeshProUGUI ItemTypeText;
    [SerializeField] private TextMeshProUGUI ItemDescriptionText;

    [SerializeField] private int DefaultFontSize;

    public void ShowToolTip(ItemDataEquipment item)
    {
        if(item == null)
        {
            return;
        }

        ItemNameText.text = item.itemName;
        ItemTypeText.text = item.equipmentType.ToString();
        ItemDescriptionText.text = item.GetDescription();

        AdjustFontSize(ItemNameText);
        AdjustPosition();


        gameObject.SetActive(true);
    }
    public void HideToolTip()
    {
        ItemNameText.fontSize = DefaultFontSize;
        gameObject.SetActive(false);
    }
}
