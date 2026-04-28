using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchWall : MonoBehaviour
{
    private Animator anim;
    public bool activation;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            ActivateWall();
        }
    }

    public void ActivateWall()
    {
        activation = true;
        anim.SetBool("transparent", true);
    }
}
