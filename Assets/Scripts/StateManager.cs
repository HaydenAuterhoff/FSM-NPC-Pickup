using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateManager<EState> : MonoBehaviour where EState : Enum
{
    protected Dictionary<EState, BaseState<EState>> States = new Dictionary<EState, BaseState<EState>>();
    protected BaseState<EState> currentState { get; set; }

    protected bool IsTransitioningState = false;

    private void Start()
    {
        currentState.EnterState();
    }

    protected virtual void Update()
    {
        EState nextStateKey = currentState.GetNextState();

        if (!IsTransitioningState && nextStateKey.Equals(currentState.stateKey))
            currentState.UpdateState();
        else if (!IsTransitioningState)
        {
            ChangeState(nextStateKey);
        }
    }

    public void ChangeState(EState stateKey)
    {
        IsTransitioningState = true;

        currentState.ExitState();
        currentState = States[stateKey];
        currentState.EnterState();

        IsTransitioningState = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        currentState.OnTriggerEnter(other);
    }

    public void OnTriggerStay(Collider other)
    {
        currentState.OnTriggerStay(other);
    }

    public void OnTriggerExit(Collider other)
    {
        currentState.OnTriggerExit(other);
    }
}
