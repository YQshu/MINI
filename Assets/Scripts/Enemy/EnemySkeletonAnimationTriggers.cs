using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeletonAnimationTriggers : MonoBehaviour
{
    private EnemySkeleton enemy => GetComponentInParent<EnemySkeleton>();

    private void AnimationTrigger()
    {
        enemy.AnimationFinshedTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRiadius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                PlayerStats target = hit.GetComponent<Player>().GetComponent<PlayerStats>();
                enemy.stats.DoDamage(target);
                //hit.GetComponent<Player>().Damage();
            }
        }
    }
    private void OpenCounterWindows() => enemy.OpenCounterAttackWindows();
    private void CloseCounterWindows()=> enemy.CloseCounterAttackWindows();

}
