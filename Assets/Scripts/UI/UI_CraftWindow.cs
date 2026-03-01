using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CraftWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDescription;
    [SerializeField] private Image itemIcon;
    [SerializeField] private Button craftButton;

    [SerializeField] private Image[] materialImage;

    public void SetupCraftWindow(ItemDataEquipment _data)
    {

        craftButton.onClick.RemoveAllListeners();

        for (int i = 0; i < materialImage.Length; i++)
        {
            materialImage[i].color = Color.clear;
            materialImage[i].GetComponentInChildren<TextMeshProUGUI>().color = Color.clear;
        }

        for(int i = 0; i < _data.craftingMaterials.Count; i++)
        {
            if(_data.craftingMaterials.Count > materialImage.Length)
            {
                Debug.Log("craftingMaterials count is more than materialImage length");
            }
            materialImage[i].sprite = _data.craftingMaterials[i].data.icon;
            materialImage[i].color = Color.white;

            TextMeshProUGUI MaterialSlottext = materialImage[i].GetComponentInChildren<TextMeshProUGUI>();

            MaterialSlottext.text = _data.craftingMaterials[i].stackSize.ToString();
            MaterialSlottext.color = Color.clear;
        }

        itemIcon.sprite = _data.icon;
        itemName.text = _data.name;
        itemDescription.text = _data.GetDescription();

        craftButton.onClick.AddListener(() => Inventory.instance.canCraft(_data, _data.craftingMaterials));
    }
}
