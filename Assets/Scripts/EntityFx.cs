using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFx : MonoBehaviour
{

    private SpriteRenderer sr;

    [Header("Flash FX")]
    [SerializeField] private float flashDuration;
    [SerializeField] private Material hitMat;
    private Material originalMat;

    [Header("Aliment colors")]
    [SerializeField] private Color[] chillColors;
    [SerializeField] private Color[] igniteColors;
    [SerializeField] private Color[] shockColors;

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalMat = sr.material;
    }

    private IEnumerator FlashFx()
    {
        sr.material = hitMat;
        Color currentColor = sr.color;
        sr.color = Color.white;

        yield return new WaitForSeconds(flashDuration);
        sr.color = currentColor;
        sr.material = originalMat;
    }

    private void RedColorBlink()
    {
        if (sr.color != Color.white)
        {
            sr.color = Color.white;
        }
        else
        {
            sr.color = Color.red;
        }

    }
    private void CancelColorChange()
    {
        CancelInvoke();
        sr.color = Color.white;
    }

    public void ChillFxFor(float _second)
    {
        InvokeRepeating("ChillColorFx", 0, .3f);
        Invoke("CancelColorChange", _second);
    }
    public void ShockFxFor(float _second)
    {
        InvokeRepeating("ShockColorFx", 0, .3f);
        Invoke("CancelColorChange", _second);
    }


    public void IgniteFxFor(float _second)
    {
        InvokeRepeating("IgniteColorFx", 0, .3f);
        Invoke("CancelColorChange", _second);
    }

    private void IgniteColorFx()
    {
        if (sr.color != igniteColors[0])
        {
            sr.color = igniteColors[0];
        }else
        {
            sr.color = igniteColors[1];
        }
    }
    private void ShockColorFx()
    {
        if (sr.color != shockColors[0])
        {
            sr.color = shockColors[0];
        }else
        {
            sr.color = shockColors[1];
        }
    }
    private void ChillColorFx()
    {
        if (sr.color != chillColors[0])
        {
            sr.color = chillColors[0];
        }
        else
        {
            sr.color = chillColors[1];
        }
    }
}
