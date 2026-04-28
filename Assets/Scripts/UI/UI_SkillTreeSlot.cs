using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillTreeSlot : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    private UI ui;

    [SerializeField] private string skillName;
    [SerializeField] private int skillCost;
    [TextArea]
    [SerializeField] private string skillDescription;
    [SerializeField] private Color lockedColor;


    public bool unlocked;

    [SerializeField] private UI_SkillTreeSlot[] shouldBeUnLocked;
    [SerializeField] private UI_SkillTreeSlot[] shouldBeLocked;

    private Image skillImage;


    private void OnValidate()
    {
        gameObject.name = "SkillTreeSlot_UI - " + skillName;
    }


    private void Start()
    {
        skillImage = GetComponent<Image>();
        skillImage.color = lockedColor;
        ui = GetComponentInParent<UI>();

        GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot());
    }

    public void UnlockSkillSlot()
    {
        for (int i = 0; i < shouldBeUnLocked.Length; i++)
        {
            if (shouldBeUnLocked[i].unlocked == false)
            {
                Debug.Log("lock");
                return;
            }
        }

        for(int i = 0;i < shouldBeLocked.Length;i++)
        {
            if(shouldBeLocked[i].unlocked == true)
            {
                Debug.Log("lock");
                return;
            }
        }

        unlocked = true;
        skillImage.color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillSlotTip.ShowToolTip(skillDescription , skillName, skillCost);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.skillSlotTip.HideToolTip();
    }
}
