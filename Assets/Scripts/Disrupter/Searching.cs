using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Searching : BaseState<DisrupterSM.DisrupterState>
{
    private DisrupterSM disrupterSM;

    private float wanderRadius = 10f; // The radius within which to wander
    private float timeToDetect = 5f; // Time to wait before transitioning to Detect
    private float timer; // Timer to track elapsed time

    private bool isTransitioning;

    public Searching(DisrupterSM _sm, GameObject _npc, NavMeshAgent _agent, DisrupterSM.DisrupterState stateKey)
        : base(stateKey) // Pass the stateKey to the base class constructor
    {
        npc = _npc;
        agent = _agent;
        disrupterSM = _sm;
    }

    public void SetDisruptorTarget(Transform pickup)
    {
        target = pickup;
        Debug.Log("Target set to: " + target);
    }

    public override void EnterState()
    {
        timer = 0f; // Reset timer
        isTransitioning = false;
        SetRandomDestination();
    }

    public override void UpdateState() 
    {
        //Debug.Log("UpdateState called. Current target: " + target);
        timer += Time.deltaTime; // Update timer

        if (timer >= timeToDetect)
        {
            SetRandomDestination();
            timer = 0;
        }

        if (target != null && Vector3.Distance(npc.transform.position, target.position) > 1f)
        {
            isTransitioning = true;
            disrupterSM.CreateStage(target);
        }
    }

    public override void ExitState()
    {
        agent.ResetPath(); // Stop wandering
    }

    public override DisrupterSM.DisrupterState GetNextState()
    {
        if (isTransitioning)
        {

            return DisrupterSM.DisrupterState.Detect;
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
