using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ItemSlot : MonoBehaviour , IPointerDownHandler ,IPointerEnterHandler , IPointerExitHandler
{
    [SerializeField] protected Image ItemImage;
    [SerializeField] protected TextMeshProUGUI ItemText;

    protected UI ui;
    public InventoryItem Item;

    protected virtual void Start()
    {
        ui = GetComponentInParent<UI>();
    }


    public void UpdateSlot(InventoryItem _newitem)
    {
        Item = _newitem;
        ItemImage.color = Color.white;

        if (Item != null)
        {
            ItemImage.sprite = Item.data.icon;
            if (Item.stackSize > 1)
            {
                ItemText.text = Item.stackSize.ToString();
            }
            else
            {
                ItemText.text = "";
            }
        }
    }

    public void CleanUpSlot()
    {
        Item = null;

        ItemImage.sprite = null;
        ItemImage.color = Color.clear;
        ItemText.text = "";
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (Item == null)
        {
            return;
        }
        if(Input.GetKey(KeyCode.LeftControl))
        {
            Inventory.instance.RemoveItem(Item.data);
            return;
        }

        if (Item.data.itemType == ItemType.Equipment)
        {
            Inventory.instance.EquipItem(Item.data);
        }
        ui.itemToolTip.HideToolTip();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(Item == null)
        {
            return;
        }
        ui.itemToolTip.ShowToolTip(Item.data as ItemDataEquipment);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(Item == null)
        {
            return;
        }
        ui.itemToolTip.HideToolTip();
    }
}
