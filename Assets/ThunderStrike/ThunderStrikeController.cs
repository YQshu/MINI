using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStrikeController : MonoBehaviour
{
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            PlayerStats playerStats = PlayerManager.Instance.player.GetComponent<PlayerStats>();
            EnemyStats enemyStats = collision.GetComponent<EnemyStats>();
            playerStats.doMagicDamage(enemyStats);
        }
    }
}
