using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public EnemyState CurrentState { get; private set; }

    public List<EnemyState> states;
    public event Action<EnemyState> stateChanged;// event to notify other objects of the state change

    public StateMachine()
    {
        states = new List<EnemyState>();
    }

    public void AddState(EnemyState state)
    {
        states.Add(state);
    }

    public void Initialize(EnemyState state)
    {
        CurrentState = state;
        CurrentState.OnEnter();
    }

    public void Update()
    {
        CurrentState.OnUpdate();
    }

    public void ChangeState(EnemyState state)
    {
        CurrentState.OnExit();
        CurrentState = state;
        CurrentState.OnEnter();

        stateChanged?.Invoke(CurrentState);
    }
}
