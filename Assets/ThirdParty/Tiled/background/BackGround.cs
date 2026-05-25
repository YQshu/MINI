using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    private GameObject cam;
    [SerializeField] private float parallaxeffect;
    private float xPosition;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera");
        xPosition = transform.position.x;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float distanceToMove = cam.transform.position.x * parallaxeffect;
        float newX = xPosition + distanceToMove;
        transform.position = new Vector3(Mathf.Round(newX * 100) / 100, transform.position.y);
    }
}