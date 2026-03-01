using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatemackine
{
    public EnemyState currentState {get;private set;}
    
    public void Initialize(EnemyState _Startstate)
    {
        currentState = _Startstate;
        currentState.Enter();
    }

    public void ChangeState(EnemyState _newstate)
    {
        currentState.Exit();
        currentState = _newstate;
        currentState.Enter();
    }
}
