using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Idle : State
{
    public float idleTimer = 3f;
    private float timer;
    public Idle(GameObject _npc, NavMeshAgent _agent) 
        : base(_npc, _agent)
    {
        name = STATE.IDLE;
    }

    public void SetPickupTarget(Transform pickup)
    {
        target = pickup;
    }

    public override void Enter()
    {
        timer = 0;
        base.Enter();
    }

    public override void Update()
    {
        timer += Time.deltaTime;

        if (target != null && Vector3.Distance(npc.transform.position, target.position) > 1f)
        {
            nextState = new MoveTo(npc, agent, target);
            stage = EVENT.EXIT;
        }

        if (timer >= idleTimer && Random.Range(0, 100) < 50)
        {
            nextState = new Patrol(npc, agent);
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
