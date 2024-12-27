using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveTo : State
{
    public MoveTo(GameObject _npc, NavMeshAgent _agent, Transform _target)
    : base(_npc, _agent, _target)
    {
        name = STATE.MOVETO;
    }

    public override void Enter()
    {
        agent.SetDestination(target.position);
        base.Enter();
    }

    public override void Update()
    {
        Vector3 direction = npc.transform.position - target.position;
        if (direction.magnitude < 2)
        {
            if (target.CompareTag("Pickup"))
            {
                nextState = new Pickup(npc, agent, target);
                stage = EVENT.EXIT;
            }
            else if (target.CompareTag("Dropoff"))
            {
                nextState = new Dropoff(npc, agent, target);
                stage = EVENT.EXIT;
            }
            else if (target.CompareTag("Reset"))
            {
                nextState = new Patrol(npc, agent);
                stage = EVENT.EXIT;
            }
        }

        //base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
