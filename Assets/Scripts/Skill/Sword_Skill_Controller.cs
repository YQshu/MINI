using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb; 
    private CircleCollider2D cd;
    private Player player;
    [SerializeField] private float Returnspeed = 12f; 

    private bool canRotate = true;
    public bool isReturning;
    private float freezeTimeDuration;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }

    public void SetupSword(Vector2 _dir , float _gravityScale, Player _player, float _freezeTimeDuration)
    {
        player = _player;
        rb.velocity = _dir;
        freezeTimeDuration = _freezeTimeDuration;
        rb.gravityScale = _gravityScale;

        anim.SetBool("Rotation",true);
    }

    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        //rb.isKinematic = false;
        transform.parent = null;   
        isReturning = true;
    }

    private void Update()
    {
        if (canRotate)
            transform.right = rb.velocity;


        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, Returnspeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, player.transform.position) < 1)
            {
                player.CatchTheSword();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning)
        {
            return;
        }
        anim.SetBool("Rotation", false);

        canRotate = false;
        cd.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        transform.parent = collision.transform;
        if(collision.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            player.stats.DoDamage(enemy.GetComponent<Characterstats>());
            enemy.FreezeTimeFor(freezeTimeDuration);

            ItemDataEquipment equipmentAmulet = Inventory.instance.GetEquipment(EquipmentType.Amulet);
            if (equipmentAmulet != null)
            {
                equipmentAmulet.ExecuteItemEffect(enemy.transform);
            }
        }
    }
}
