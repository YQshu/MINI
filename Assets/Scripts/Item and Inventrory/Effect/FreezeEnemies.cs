using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Freeze enemies Effect", menuName = "Data/Item Effect/Freeze enemies Effect")]
public class FreezeEnemies : ItemEffect
{
    [SerializeField] private float duration;



    public override void ExecuteEffect(Transform _enemyTransform)
    {
        PlayerStats playerStats = _enemyTransform.GetComponent<PlayerStats>();

        if (playerStats.currentHp > playerStats.GetMaxHp() * .1f)
        {
            return;
        }

        if(!Inventory.instance.CanUseArmor())
        {
            return;
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_enemyTransform.position, 2);

        foreach(var hit in colliders)
        {
            hit.GetComponent<Enemy>()?.FreezeTimeFor(duration);
        }
    }
}
