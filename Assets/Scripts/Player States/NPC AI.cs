using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_AI : MonoBehaviour
{
    NavMeshAgent agent;
    State currentState;

    [SerializeField] FloatingText stateText;

    private void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        currentState = new Idle(this.gameObject, agent);
    }

    private void Update()
    {
        currentState = currentState.Process();
        stateText.DisplayState(currentState.name.ToString());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickup") && currentState is Idle idleState)
        {
            idleState.SetPickupTarget(other.transform);
        }
        else if (other.CompareTag("Pickup") && currentState is Patrol patrolState)
        {
            patrolState.SetPickupTarget(other.transform);
        }
    }
}
