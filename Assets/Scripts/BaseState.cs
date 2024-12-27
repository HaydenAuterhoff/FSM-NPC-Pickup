using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseState<EState> where EState : Enum
{
    public EState stateKey { get; private set; }
    public BaseState(EState key)
    {
        stateKey = key;
    }

    protected GameObject npc;
    protected NavMeshAgent agent;
    protected Transform target;

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract EState GetNextState();

    public abstract void OnTriggerEnter(Collider other);
    public abstract void OnTriggerStay(Collider other);
    public abstract void OnTriggerExit(Collider other);
}
