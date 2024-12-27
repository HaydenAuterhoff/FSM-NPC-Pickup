using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wander : BaseState<SupporterSM.SupporterStates>
{
    private float wanderRadius = 10f; // The radius within which to wander
    private float timeToDetect = 5f; // Time to wait before transitioning to Detect
    private float timer; // Timer to track elapsed time
    private bool isTransitioning;
    private bool isEventThrowing;

    private DisrupterSM disrupterSM;

    public Wander(GameObject _npc, NavMeshAgent _agent, SupporterSM.SupporterStates stateKey, DisrupterSM _disrupterSM)
        : base(stateKey)
    {
        npc = _npc;
        agent = _agent;
        disrupterSM = _disrupterSM;
    }

    public override void EnterState()
    {
        timer = 0f;
        isTransitioning = false;
        SetRandomDestination();
    }

    public override void UpdateState()
    {
        timer += Time.deltaTime; // Update timer

        if (timer >= timeToDetect)
        {
            SetRandomDestination();
            timer = 0;
        }

        if (isEventThrowing)
        {
            isTransitioning = true;
            isEventThrowing = false;
        }
    }

    public override void ExitState()
    {
        agent.ResetPath();
    }

    public void HandleThrowingStateEntered()
    {
        isEventThrowing = true;
    }

    public override SupporterSM.SupporterStates GetNextState()
    {
        if (isTransitioning)
        {

            return SupporterSM.SupporterStates.Cheer;
        }

        return stateKey;
    }

    private void SetRandomDestination()
    {
        //Generate a random point within the specified radius
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += npc.transform.position; //Center the random point around the NPC

        //Check if the random point is on the NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position); //Set destination to valid NavMeshPosition
        }
        else
        {
            SetRandomDestination(); //Do it again --> maybe set to Idle
        }
    }

    public override void OnTriggerEnter(Collider other) { }

    public override void OnTriggerExit(Collider other) { }

    public override void OnTriggerStay(Collider other) { }
}
