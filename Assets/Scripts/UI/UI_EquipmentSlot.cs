using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EquipmentSlot : UI_ItemSlot
{
    public EquipmentType slotType;

    private void OnValidate()
    {
        gameObject.name = "Equipment_Slot -" + slotType.ToString();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if(Item == null || Item.data == null)
        {
            return;
        }

        Inventory.instance.UnequipItem(Item.data as ItemDataEquipment);
        Inventory.instance.AddItem(Item.data as ItemDataEquipment);

        ui.itemToolTip.HideToolTip();

        CleanUpSlot();
    }
}
