using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffect : ScriptableObject
{
    [TextArea]
    public string Effectdescription;

    public virtual void ExecuteEffect(Transform _enemyTransform)
    {
        Debug.Log("Item Effect Executed");
    }
}
