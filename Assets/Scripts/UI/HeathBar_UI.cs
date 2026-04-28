using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HeathBar_UI : MonoBehaviour
{
    private Entity entity;
    private RectTransform myTransform;
    private Slider slider;
    private Characterstats myStats;

    private void Start()
    {

        myTransform = GetComponent<RectTransform>();
        entity = GetComponentInParent<Entity>();
        slider = GetComponentInChildren<Slider>();
        myStats = GetComponentInParent<Characterstats>();

        entity.onFipped += FilpUI;
        myStats.onHeathChanged += UpdateHeathUI;
    }
    private void Update()
    {
        UpdateHeathUI();
    }

    private void UpdateHeathUI()
    {
        slider.maxValue = myStats.GetMaxHp();
        slider.value = myStats.currentHp;
    }


    private void FilpUI()
    {
        myTransform.Rotate(0, 180, 0);
    }

    private void OnDisable()
    {
        entity.onFipped -= FilpUI;
        myStats.onHeathChanged -= UpdateHeathUI;
    }
}
