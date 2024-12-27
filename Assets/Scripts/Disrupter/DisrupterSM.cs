using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class DisrupterSM : StateManager<DisrupterSM.DisrupterState>
{
    public event Action OnThrowingStateEntered;
    public enum DisrupterState
    {
        Searching,
        Detect,
        Throwing
    };

    [SerializeField] FloatingText stateText;

    protected GameObject npc;
    protected NavMeshAgent agent;


    private void Awake()
    {
        npc = this.gameObject;
        agent = this.GetComponent<NavMeshAgent>();
        InitializeStates();
    }

    public void SetDisruptTarget(Transform pickup)
    {      
        if (currentState is Searching searchingState)
        {
            searchingState.SetDisruptorTarget(pickup.transform);
        }
    }

    protected override void Update()
    {
        base.Update();
        stateText.DisplayState(currentState.ToString());
    }

    private void InitializeStates()
    {
        Searching searchState = new Searching(this, npc, agent, DisrupterState.Searching);
        States.Add(DisrupterState.Searching, searchState);

        currentState = States[DisrupterState.Searching];
    }

    public void CreateStage(Transform target)
    {
        if (currentState is Searching searchingState)
        {
            if (!States.ContainsKey(DisrupterState.Detect))
            {
                Detect newDetectState = new Detect(this, npc, agent, target, DisrupterState.Detect);
                States.Add(DisrupterState.Detect, newDetectState);
            }
            else
            {
                if (States[DisrupterState.Detect] is Detect oldDetectState)
                {
                    oldDetectState.SetDisruptorTarget(target);
                }
            }
        }

        if (currentState is Detect detectState)
        {
            if (!States.ContainsKey(DisrupterState.Throwing))
            {
                Throwing newThrowingState = new Throwing(this, npc, agent, target, DisrupterState.Throwing);
                States.Add(DisrupterState.Throwing, newThrowingState);
            }
            else
            {
                if (States[DisrupterState.Throwing] is Throwing oldThrowingState)
                {
                    oldThrowingState.SetDisruptorTarget(target);
                }
            }
        }

        if (States[DisrupterState.Searching] is Searching oldSearchingState)
        {
            oldSearchingState.SetDisruptorTarget(target);
        }
    }

    public void ChangeState(DisrupterState newState)
    {
        if (newState == DisrupterState.Throwing)
        {
            OnThrowingStateEntered?.Invoke();
        }
    }
}
