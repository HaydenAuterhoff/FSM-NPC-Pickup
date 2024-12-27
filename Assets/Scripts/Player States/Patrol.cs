using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : State
{
    public float patrolRadius = 10f;
    public float patrolTimer = 5f;
    private float timer;

    public Patrol(GameObject _npc, NavMeshAgent _agent)
    : base(_npc, _agent)
    {
        name = STATE.PATROL;
    }

    public void SetPickupTarget(Transform pickup)
    {
        target = pickup;
    }

    public override void Enter()
    {
        timer = 0; //Reset the timer when entering state
        SetRandomDestination();
        base.Enter();
    }

    public override void Update()
    {
        timer += Time.deltaTime;

        if (timer >= patrolTimer)
        {
            SetRandomDestination();
            timer = 0;
        }

        if (target != null && Vector3.Distance(npc.transform.position, target.position) > 1f)
        {
            nextState = new MoveTo(npc, agent, target);
            stage = EVENT.EXIT;
        }

        if (timer >= patrolTimer && Random.Range(0, 100) < 10)
        {
            nextState = new Idle(npc, agent);
            stage = EVENT.EXIT;
        }

        //base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }

    private void SetRandomDestination()
    {
        //Generate a random point within the specified radius
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += npc.transform.position; //Center the random point around the NPC

        //Check if the random point is on the NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position); //Set destination to valid NavMeshPosition
        }
        else
        {
            SetRandomDestination(); //Do it again --> maybe set to Idle
        }
    }
}
