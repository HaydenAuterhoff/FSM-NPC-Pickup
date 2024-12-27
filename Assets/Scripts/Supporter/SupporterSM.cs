using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SupporterSM : StateManager<SupporterSM.SupporterStates>
{
    public enum SupporterStates
    {
        Wander,
        Cheer
    };

    [SerializeField] FloatingText stateText;

    protected GameObject npc;
    protected NavMeshAgent agent;
    private DisrupterSM disrupterSM;

    private void Awake()
    {
        npc = this.gameObject;
        agent = this.GetComponent<NavMeshAgent>();
        disrupterSM = FindAnyObjectByType<DisrupterSM>();

        InitializeStates();

        if (disrupterSM != null)
        {
            disrupterSM.OnThrowingStateEntered += HandleThrowingStateEntered;
        }
    }

    protected override void Update()
    {
        base.Update();
        stateText.DisplayState(currentState.ToString());
    }

    private void InitializeStates()
    {
        Wander wanderState = new Wander(npc, agent, SupporterStates.Wander, disrupterSM);
        States.Add(SupporterStates.Wander, wanderState);
        Cheer cheerState = new Cheer(npc, agent, SupporterStates.Cheer);
        States.Add(SupporterStates.Cheer, cheerState);

        currentState = States[SupporterStates.Wander];
    }

    private void HandleThrowingStateEntered()
    {
        // Notify the current Wander state
        if (currentState is Wander wanderState)
        {
            wanderState.HandleThrowingStateEntered();
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from the event to prevent memory leaks
        if (disrupterSM != null)
        {
            disrupterSM.OnThrowingStateEntered -= HandleThrowingStateEntered;
        }
    }
}
