using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftSlot : UI_ItemSlot
{
    protected override void Start()
    {
        base.Start();
    }

    public void SetupCraftSlot(ItemDataEquipment _Data)
    {
        if (_Data == null)
        {
            return;
        }
        Item.data = _Data;

        ItemImage.sprite = _Data.icon;
        ItemText.text = _Data.itemName;
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        ui.craftWindow.SetupCraftWindow(Item.data as ItemDataEquipment);
    }
}
