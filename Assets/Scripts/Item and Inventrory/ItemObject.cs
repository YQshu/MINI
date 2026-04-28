using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ItemData itemdata;


    private void SetupVisual()
    {
        if (itemdata == null)
        {
            return;
        }
        GetComponent<SpriteRenderer>().sprite = itemdata.icon;
        gameObject.name = "Item object - " + itemdata.name;

    }

    public void SetupItem(ItemData _itemdata, Vector2 _velocity)
    {
        itemdata = _itemdata;
        rb.velocity = _velocity;
        SetupVisual();
    }


    public void PickUpItem()
    {
        if(Inventory.instance.CanAddItem() && itemdata.itemType == ItemType.Equipment)
        {
            rb.velocity = new Vector2(0, 7);
            return;
        }

        Inventory.instance.AddItem(itemdata);
        Destroy(gameObject);
    }
}