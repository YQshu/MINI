using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Thunder Strike Effect", menuName = "Data/Item Effect/ThunderStrike Effect")]
public class ThunderStrike_Effect : ItemEffect
{
    [SerializeField] private GameObject thunderStrikeEffect;

    public override void ExecuteEffect(Transform _enemyTransform)
    {
        GameObject newThunderStrikeEffect = Instantiate(thunderStrikeEffect,_enemyTransform.position,Quaternion.identity);
        Destroy(newThunderStrikeEffect, 1);
    }
}
