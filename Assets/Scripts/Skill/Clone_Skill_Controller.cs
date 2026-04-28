using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{
    private Player player;
    private SpriteRenderer sr;
    private Animator anim;
    [SerializeField] private float colorloosingSpeed;
    private float cloneTimer;
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRiadius = .8f;
    private Transform closetEnemy;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        cloneTimer -= Time.deltaTime;
        if (cloneTimer <= 0)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * colorloosingSpeed));

            if (sr.color.a <= 0)
            {
                Destroy(gameObject);
            }
        }

    }
    public void SetupClone(Transform _newTransform, float _cloneDuration,bool _canAttack,Player _player)
    {
        if(_canAttack)
        {
            anim.SetInteger("AttackNumber", Random .Range(1, 4));
        }
        player = _player;
        transform.position = _newTransform.position;
        cloneTimer = _cloneDuration;

        FaceClosesTarget();
    }
    private void AnimationTrigger()
    {
        cloneTimer = -.1f;
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRiadius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                hit.GetComponent<Entity>().SetupKnockbackDir(transform);
                player.stats.DoDamage(hit.GetComponent<Characterstats>());
            }
        }
    }
    private void FaceClosesTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);
        float closesDistance = Mathf.Infinity;

        foreach (var hit in colliders)
        {
            if(hit.GetComponent<Enemy>()!= null)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);
                if (distanceToEnemy < closesDistance)
                {
                    closetEnemy = hit.transform;
                    closesDistance = distanceToEnemy;
                }
            }
        }
        if(closetEnemy!= null)
        {
            if(transform.position.x > closetEnemy.position.x)
            {
                transform.Rotate(0, 180, 0); 
            }
        }

    }
}
