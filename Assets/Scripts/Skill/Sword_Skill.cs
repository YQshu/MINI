using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill : Skill
{
    [Header("Skill info")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchForce;
    [SerializeField] private float swordGravity;
    [SerializeField] private float freezeTimeDuration;

    private Vector2 finalDir;

    [Header("Aim dots")]
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBeetweenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsParent;

    private GameObject[] dots;

    protected override void Start()
    {
        base.Start();
        GenerateDots();
    }

    protected override void Update()
    {
        if(Input.GetKeyUp(KeyCode.Mouse1))
        {
            finalDir = new Vector2(AimDiretion().normalized.x * launchForce.x, AimDiretion().normalized.y * launchForce.y);
        }
        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = DotsPositions(i * spaceBeetweenDots);
            }
        }

    }

    public void CreateSword()
    {    
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        Sword_Skill_Controller newSwordScript = newSword.GetComponent<Sword_Skill_Controller>();
        newSwordScript.SetupSword(finalDir, swordGravity, player,freezeTimeDuration);
        player.AssignNewSword(newSword);
        DotsActive(false);
    }

    public Vector2 AimDiretion()
    {
        Vector2 playerPostion = player.transform.position;
        Vector2 mousePostion = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 diretion = mousePostion - playerPostion;
        return diretion;
    }

    public void DotsActive(bool _isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(_isActive);
        }
    }

    private void GenerateDots()
    {
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false);
        }
    }


    private Vector2 DotsPositions(float t)
    {
        Vector2 positions = (Vector2)player.transform.position + new Vector2(
            AimDiretion().normalized.x * launchForce.x,
            AimDiretion().normalized.y * launchForce.y) * t + .5f*(Physics2D.gravity.normalized * swordGravity) * ( t * t );

        return positions;
    }
}
